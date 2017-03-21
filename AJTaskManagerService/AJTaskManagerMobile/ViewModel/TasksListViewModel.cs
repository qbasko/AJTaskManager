using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.ViewModel
{
    public class TasksListViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly ITaskItemDataService _taskItemDataService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private readonly IUserDataService _userDataService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private ObservableCollection<TaskItem> _taskItems;
        private string _userInternalId;

        private RelayCommand _addNewItemCommand;
        private RelayCommand _deleteCompletedCommand;
        private RelayCommand<object> _navigateToTaskItemsCommand;
        private RelayCommand<object> _editTaskItemCommand;
        private RelayCommand<object> _checkboxCheckedCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand<object> _deleteTaskCommand;

        public ObservableCollection<TaskItem> TaskItems
        {
            get { return _taskItems; }
            set
            {
                if (_taskItems == value)
                    return;
                _taskItems = value;
                RaisePropertyChanged(() => TaskItems);
            }
        }

        private bool _isBusy;
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

        public RelayCommand AddNewItemCommand
        {
            get
            {
                return _addNewItemCommand ??
                       (_addNewItemCommand = new RelayCommand(() =>
                       {
                           _navigationService.NavigateTo(Constants.TaskItemFormKey);
                       }));
            }
        }

        public RelayCommand<object> DeleteTaskCommand
        {
            get
            {
                return _deleteTaskCommand ??
                       (_deleteTaskCommand = new RelayCommand<object>(async (param) =>
                       {
                           IsBusy = true;
                           TaskItem taskItem = param as TaskItem;
                           if (taskItem != null)
                           {
                               string userId = await GetUserInternalId();
                               bool canUserDelete = await _roleTypeDataService.CanUserAddOrDeleteItem(userId, taskItem.GroupId);
                               if (canUserDelete)
                               {
                                   taskItem.IsDeleted = true;
                                   await _taskItemDataService.UpdateTaskItem(taskItem);
                                   //Update taskSubitems
                                   var taskSubitems = await _taskSubitemDataService.GetTaskSubitems(taskItem.Id);
                                   taskSubitems.ForEach(async ts =>
                                   {
                                       ts.IsDeleted = true;
                                       await _taskSubitemDataService.UpdateTaskSubitem(ts);
                                   });
                                  await Refresh();
                               }
                           }
                           IsBusy = false;
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
                    var completedTasks = _taskItems.Where(t => t.IsCompleted);
                    string userId = await GetUserInternalId();
                    bool canUserDelete = false;
                    foreach (var completedTask in completedTasks)
                    {
                        bool result = await _roleTypeDataService.CanUserAddOrDeleteItem(userId, completedTask.GroupId);
                        if (!result)
                            return;
                        canUserDelete = true;
                    }
                    completedTasks.ForEach(async t =>
                    {
                        if (canUserDelete)
                        {
                            IsBusy = true;
                            t.IsDeleted = true;
                            await _taskItemDataService.UpdateTaskItem(t);
                            //Update taskSubitems
                            var taskSubitems = await _taskSubitemDataService.GetTaskSubitems(t.Id);
                            taskSubitems.ForEach(async ts =>
                            {
                                ts.IsDeleted = true;
                                await _taskSubitemDataService.UpdateTaskSubitem(ts);
                            });
                            Refresh();
                        }
                        else
                        {
                            IsBusy = false;
                            new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                        }
                    });
                }));
            }
        }

        public RelayCommand<object> NavigateToTaskItemsCommand
        {
            get
            {
                return _navigateToTaskItemsCommand ?? (
                    _navigateToTaskItemsCommand = new RelayCommand<object>((obj) =>
                    {
                        _navigationService.NavigateTo(Constants.TaskSubitemsPageKey, obj);
                    }));
            }
        }

        public RelayCommand<object> EditTaskItemCommand
        {
            get
            {
                return _editTaskItemCommand ??
                       (_editTaskItemCommand = new RelayCommand<object>((obj) =>
                       {
                           _navigationService.NavigateTo(Constants.TaskItemFormKey, obj);
                       }));
            }
        }

        public RelayCommand<object> CheckboxCheckedCommand
        {
            get
            {
                return _checkboxCheckedCommand ??
                       (_checkboxCheckedCommand = new RelayCommand<object>(async (obj) =>
                       {
                           var userInternalId = await GetUserInternalId();
                           TaskItem taskItem = obj as TaskItem;
                           if (taskItem != null)
                           {
                               //IsBusy = true;
                               bool canUserUpdate = await _roleTypeDataService.CanUserEditItem(userInternalId, taskItem.GroupId);
                               if (canUserUpdate)
                                   await _taskItemDataService.UpdateTaskItem(taskItem);
                               else
                                   Refresh();
                           }
                       }));
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ??
                       (_refreshCommand = new RelayCommand(async ()=>await Refresh()));
            }
        }

        public TasksListViewModel(INavigationService navigationService, ITaskItemDataService taskItemDataService, IUserDataService userDataService, IRoleTypeDataService roleTypeDataService, ITaskSubitemDataService taskSubitemDataService)
        {
            _navigationService = navigationService;
            _taskItemDataService = taskItemDataService;
            _userDataService = userDataService;
            _roleTypeDataService = roleTypeDataService;
            _taskSubitemDataService = taskSubitemDataService;
            Messenger.Default.Register<Group>(this, HandleMessage);
            // Refresh();
        }

        private void HandleMessage(Group obj)
        {
        }

        private async Task Refresh()
        {
            try
            {
                IsBusy = true;
                var userId = AccountHelper.GetCurrentUserId();
                string userInternalId = await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                TaskItems = await _taskItemDataService.GetTaskItems(userInternalId);
                TaskItems = TaskItems.OrderBy(t => t.Name).ToObservableCollection();
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
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
            Refresh();
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
