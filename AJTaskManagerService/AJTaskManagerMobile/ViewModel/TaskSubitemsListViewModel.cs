using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.ViewModel
{
    public class TaskSubitemsListViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private readonly IUserDataService _userDataService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private ObservableCollection<TaskSubitem> _taskSubitems;
        private string _taskName;
        private bool _isBusy;
        private string _userInternalId;
        private TaskItem _associatedTaskItem;
        private RelayCommand<object> _addNewItemCommand;
        private RelayCommand _deleteCompletedCommand;
        private RelayCommand<object> _navigateToTaskSubitemCommand;
        private RelayCommand<object> _checkbxoxCheckedCommand;
        private RelayCommand<object> _navigateToTaskSubitemWorkCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand<object> _deleteTaskSubitemCommand;

        public ObservableCollection<TaskSubitem> TaskSubitems
        {
            get { return _taskSubitems; }
            set
            {
                if (_taskSubitems == value)
                    return;
                _taskSubitems = value;
                RaisePropertyChanged(() => TaskSubitems);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public string TaskName
        {
            get { return _taskName; }
            set
            {
                if (_taskName == value)
                    return;
                _taskName = value;
                RaisePropertyChanged(() => TaskName);
            }
        }

        public RelayCommand<object> AddNewItemCommand
        {
            get
            {
                return _addNewItemCommand ??
                       (_addNewItemCommand = new RelayCommand<object>(async obj =>
                       {
                           IsBusy = true;
                           var internalUserId = await GetUserInternalId();
                           bool canUserAdd =
                               await
                                   _roleTypeDataService.CanUserAddOrDeleteItem(internalUserId,
                                       _associatedTaskItem.GroupId);
                           if (canUserAdd)
                           {
                               Tuple<TaskItem, TaskSubitem> param = new Tuple<TaskItem, TaskSubitem>(
                                   _associatedTaskItem, obj as TaskSubitem);
                               _navigationService.NavigateTo(Constants.TaskSubitemFormKey, param);
                           }
                           else
                           {
                               new MessageDialog(Constants.UserCantAdd).ShowAsync();
                           }
                           IsBusy = false;
                       }));
            }
        }

        public RelayCommand<object> DeleteTaskSubitemCommand
        {
            get
            {
                return _deleteTaskSubitemCommand ??
                       (_deleteTaskSubitemCommand = new RelayCommand<object>(async (param) =>
                       {
                           TaskSubitem taskSubitem = param as TaskSubitem;
                           if (taskSubitem != null)
                           {
                               string userId = await GetUserInternalId();
                               bool canUserDelete =
                                   await
                                       _roleTypeDataService.CanUserAddOrDeleteItem(userId, _associatedTaskItem.GroupId);
                               if (canUserDelete)
                               {
                                   IsBusy = true;
                                   taskSubitem.IsDeleted = true;
                                   await _taskSubitemDataService.UpdateTaskSubitem(taskSubitem);
                                   Refresh();
                               }
                               else
                               {
                                   IsBusy = false;
                                   new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                               }
                           }
                       }));
            }
        }

        public RelayCommand DeleteCompletedCommand
        {
            get
            {
                return _deleteCompletedCommand ??
                       (_deleteCompletedCommand = new RelayCommand(async () =>
                       {
                           string userId = await GetUserInternalId();
                           bool canUserDelete = await _roleTypeDataService.CanUserAddOrDeleteItem(userId, _associatedTaskItem.GroupId);
                           if (canUserDelete)
                           {
                               _taskSubitems.Where(t => t.IsCompleted).ForEach(async t =>
                               {
                                   IsBusy = true;
                                   t.IsDeleted = true;
                                   await _taskSubitemDataService.UpdateTaskSubitem(t);
                                   Refresh();
                               });
                           }
                           else
                           {
                               IsBusy = false;
                               new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                           }
                       }));
            }
        }

        public RelayCommand<object> NavigateToTaskSubItemCommand
        {
            get
            {
                return _navigateToTaskSubitemCommand ??
                       (_navigateToTaskSubitemCommand = new RelayCommand<object>(obj =>
                       {
                           Tuple<TaskItem, TaskSubitem> param = new Tuple<TaskItem, TaskSubitem>(_associatedTaskItem, obj as TaskSubitem);
                           _navigationService.NavigateTo(Constants.TaskSubitemFormKey, param);
                       }));
            }
        }

        public RelayCommand<object> CheckboxCheckedCommand
        {
            get
            {
                return _checkbxoxCheckedCommand ??
                       (_checkbxoxCheckedCommand = new RelayCommand<object>(async obj =>
                       {
                           var userInternalId = await GetUserInternalId();
                           TaskSubitem taskSubitem = obj as TaskSubitem;
                           if (taskSubitem != null)
                           {
                               bool canUserUpdate = await _roleTypeDataService.CanUserEditItem(userInternalId, _associatedTaskItem.GroupId);
                               if (canUserUpdate)
                                   await _taskSubitemDataService.UpdateTaskSubitem(taskSubitem);
                               else
                                   Refresh();
                           }
                       }));
            }
        }

        public RelayCommand<object> NavigateToTaskSubitemWorkCommand
        {
            get
            {
                return _navigateToTaskSubitemWorkCommand ??
                    (_navigateToTaskSubitemWorkCommand = new RelayCommand<object>(obj =>
                    {
                        _navigationService.NavigateTo(Constants.TaskSubitemWorkPageKey, obj);
                    }));
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand??
                    (_refreshCommand = new RelayCommand(Refresh));
            }
        }

        public TaskSubitemsListViewModel(INavigationService navigationService, ITaskSubitemDataService taskSubitemDataService,
            IUserDataService userDataService, IRoleTypeDataService roleTypeDataService)
        {
            _navigationService = navigationService;
            _taskSubitemDataService = taskSubitemDataService;
            _userDataService = userDataService;
            _roleTypeDataService = roleTypeDataService;
        }

        private async void Refresh()
        {
            IsBusy = true;
            if (_associatedTaskItem != null)
            {
                TaskName = _associatedTaskItem.Name;
                TaskSubitems = await _taskSubitemDataService.GetTaskSubitems(_associatedTaskItem.Id);
                TaskSubitems = TaskSubitems.OrderBy(t => t.Name).ToObservableCollection();
            }
            IsBusy = false;
        }
        private async Task<string> GetUserInternalId()
        {
            if (!String.IsNullOrWhiteSpace(_userInternalId))
                return _userInternalId;
            _userInternalId = await _userDataService.GetUserInternalId(AccountHelper.GetCurrentUserId(), Constants.MainAuthenticationDomain);
            return _userInternalId;
        }

        public void Activate(object parameter)
        {
            _associatedTaskItem = parameter as TaskItem;
            Refresh();
        }

        public void Deactivate(object parameter)
        {
        }
    }
}
