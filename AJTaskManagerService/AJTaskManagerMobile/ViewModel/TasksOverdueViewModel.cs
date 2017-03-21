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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;

namespace AJTaskManagerMobile.ViewModel
{
    public class TasksOverdueViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly IUserDataService _userDataService;
        private readonly ITaskItemDataService _taskItemDataService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private readonly ITaskSubitemWorkDataService _taskSubitemWorkDataService;
        private RelayCommand<object> _navigateToTaskSubitemCommand;
        private RelayCommand<object> _checkbxoxCheckedCommand;
        private RelayCommand<object> _navigateToTaskSubitemWorkCommand;


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

        private bool _isRefreshVisible;
        public bool IsRefreshVisible
        {
            get { return _isRefreshVisible; }
            set
            {
                if (_isRefreshVisible == value)
                    return;
                _isRefreshVisible = value;
                RaisePropertyChanged(() => IsRefreshVisible);
            }
        }

        private ObservableCollection<TaskSubitem> _taskSubitems;
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

        public RelayCommand<object> NavigateToTaskSubitemCommand
        {
            get
            {
                return _navigateToTaskSubitemCommand ??
                    (_navigateToTaskSubitemCommand = new RelayCommand<object>(async obj =>
                    {
                        var taskSubitem = obj as TaskSubitem;
                        if (taskSubitem != null)
                        {
                            var associatedTaskItem = await _taskItemDataService.GetTaskItemById(taskSubitem.TaskItemId);
                            Tuple<TaskItem, TaskSubitem> param = new Tuple<TaskItem, TaskSubitem>(associatedTaskItem, taskSubitem);
                            _navigationService.NavigateTo(Constants.TaskSubitemFormKey, param);
                        }
                        Initialize();
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
                        Initialize();
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
                        TaskSubitem taskSubitem = obj as TaskSubitem;
                        if (taskSubitem != null)
                        {
                            await _taskSubitemDataService.UpdateTaskSubitem(taskSubitem);
                        }
                    }));
            }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ??
                       (_refreshCommand = new RelayCommand(Initialize));
            }
        }

        public TasksOverdueViewModel(INavigationService navigationService, IUserDataService userDataService, ITaskItemDataService taskItemDataService, ITaskSubitemDataService taskSubitemDataService, ITaskSubitemWorkDataService taskSubitemWorkDataService)
        {
            _navigationService = navigationService;
            _userDataService = userDataService;
            _taskItemDataService = taskItemDataService;
            _taskSubitemDataService = taskSubitemDataService;
            _taskSubitemWorkDataService = taskSubitemWorkDataService;
            TaskSubitems = new ObservableCollection<TaskSubitem>();
            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                IsBusy = true;
                IsRefreshVisible = false;
                string userId = AccountHelper.GetCurrentUserId();
                string userInternalId =
                    await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                var taskItems = await _taskItemDataService.GetTaskItems(userInternalId);
                TaskSubitems = new ObservableCollection<TaskSubitem>();
                foreach (var taskItem in taskItems)
                {
                    var taskSubitems = await _taskSubitemDataService.GetTaskSubitems(taskItem.Id);
                    Func<TaskSubitem, bool> func = t => t.TaskStatusId == ((int)TaskStatusEnum.InProgress).ToString() &&
                        t.EndDateTime.HasValue &&
                        t.EndDateTime.Value.Date <= DateTime.Today;
                    taskSubitems.Where(func).ForEach(t => TaskSubitems.Add(t));
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }
            IsBusy = false;
            IsRefreshVisible = true;
        }

        public void Activate(object parameter)
        {
            TaskSubitems = new ObservableCollection<TaskSubitem>();
            Initialize();
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
