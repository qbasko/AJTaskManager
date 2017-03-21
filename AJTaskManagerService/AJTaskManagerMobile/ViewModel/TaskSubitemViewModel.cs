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
using TaskStatus = AJTaskManagerMobile.Model.DTO.TaskStatus;

namespace AJTaskManagerMobile.ViewModel
{
    public class TaskSubitemViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private readonly ITaskItemDataService _taskItemDataService;
        private readonly IUserDataService _userDataService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private TaskItem _associatedTaskItem;
        private TaskSubitem _associatedTaskSubitem;
        private string _userInternalId;

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

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private ObservableCollection<User> _availableGroupUsers;
        public ObservableCollection<User> AvailableGroupUsers
        {
            get { return _availableGroupUsers; }
            set
            {
                if (_availableGroupUsers == value)
                    return;
                _availableGroupUsers = value;
                RaisePropertyChanged(() => AvailableGroupUsers);
            }
        }

        private User _selectedExecutor;
        public User SelectedExecutor
        {
            get { return _selectedExecutor; }
            set
            {
                if (_selectedExecutor == value)
                    return;
                _selectedExecutor = value;
                RaisePropertyChanged(() => SelectedExecutor);
            }
        }

        private ObservableCollection<TaskStatus> _availableTaskStatuses;
        public ObservableCollection<TaskStatus> AvailableTaskStatuses
        {
            get { return _availableTaskStatuses; }
            set
            {
                if (_availableTaskStatuses == value)
                    return;
                _availableTaskStatuses = value;
                RaisePropertyChanged(() => AvailableTaskStatuses);
            }
        }

        private TaskStatus _selectedTaskStatus;
        public TaskStatus SelectedTaskStatus
        {
            get { return _selectedTaskStatus; }
            set
            {
                if (_selectedTaskStatus == value)
                    return;
                _selectedTaskStatus = value;
                RaisePropertyChanged(() => SelectedTaskStatus);
            }
        }

        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set
            {
                if (_startDateTime == value)
                    return;
                _startDateTime = value;
                RaisePropertyChanged(() => StartDateTime);
            }
        }

        private DateTime _endDateTime;
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set
            {
                if (_endDateTime == value)
                    return;
                _endDateTime = value;
                RaisePropertyChanged(() => EndDateTime);
            }
        }

        private double _estimationInHours;
        public double EstimationInHours
        {
            get { return _estimationInHours; }
            set
            {
                if (_estimationInHours == value)
                    return;
                _estimationInHours = value;
                RaisePropertyChanged(() => EstimationInHours);
            }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (_isDeleted == value)
                    return;
                _isDeleted = value;
                RaisePropertyChanged(() => IsDeleted);
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted == value)
                    return;
                _isCompleted = value;
                RaisePropertyChanged(() => IsCompleted);
            }
        }

        private ObservableCollection<TaskSubitem> _availableTaskSubitems;
        public ObservableCollection<TaskSubitem> AvailableTaskSubitems
        {
            get { return _availableTaskSubitems; }
            set
            {
                if (_availableTaskSubitems == value)
                    return;
                _availableTaskSubitems = value;
                RaisePropertyChanged(() => AvailableTaskSubitems);
            }
        }

        private TaskSubitem _selectedPredecessor;
        public TaskSubitem SelectedPredecessor
        {
            get { return _selectedPredecessor; }
            set
            {
                if (_selectedPredecessor == value)
                    return;
                _selectedPredecessor = value;
                RaisePropertyChanged(() => SelectedPredecessor);
            }
        }

        private TaskSubitem _selectedSuccessor;
        public TaskSubitem SelectedSuccessor
        {
            get { return _selectedSuccessor; }
            set
            {
                if (_selectedSuccessor == value)
                    return;
                _selectedSuccessor = value;
                RaisePropertyChanged(() => SelectedSuccessor);
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (
                    _saveCommand = new RelayCommand(async () =>
                    {
                        IsBusy = true;
                        var internalUserId = await GetUserInternalId();
                        TaskSubitem taskSubitem = new TaskSubitem();
                        taskSubitem.Id = Id;
                        taskSubitem.Name = Name;
                        taskSubitem.Description = Description;
                        taskSubitem.TaskStatusId = SelectedTaskStatus.Id;
                        taskSubitem.StartDateTime = StartDateTime;
                        taskSubitem.EndDateTime = EndDateTime;
                        taskSubitem.EstimationInHours = EstimationInHours;
                        taskSubitem.IsCompleted = false;
                        taskSubitem.IsDeleted = false;
                        taskSubitem.PredecessorId = SelectedPredecessor != null ? SelectedPredecessor.Id : null;
                        taskSubitem.SuccessorId = SelectedSuccessor != null ? SelectedSuccessor.Id : null;
                        taskSubitem.TaskItemId = _associatedTaskItem.Id;
                        taskSubitem.ExecutorId = SelectedExecutor != null ? SelectedExecutor.Id : null;

                        if (String.IsNullOrWhiteSpace(taskSubitem.Id))
                        {
                            bool canUserAdd = await _roleTypeDataService.CanUserAddOrDeleteItem(internalUserId, _associatedTaskItem.GroupId);
                            if (canUserAdd)
                            {
                                taskSubitem.Id = Guid.NewGuid().ToString();
                                await _taskSubitemDataService.InsertTaskSubitem(taskSubitem);
                                IsBusy = false;
                                _navigationService.GoBack();
                            }
                            else
                                new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                        }
                        else
                        {
                            bool canUserEdit = await _roleTypeDataService.CanUserEditItem(internalUserId, _associatedTaskItem.GroupId);
                            if (canUserEdit)
                            {
                                await _taskSubitemDataService.UpdateTaskSubitem(taskSubitem);
                                IsBusy = false;
                                _navigationService.GoBack();
                            }
                            else
                                new MessageDialog(Constants.UserCantEdit).ShowAsync();
                        }
                        IsBusy = false;
                    }));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = new RelayCommand(() =>
                       {
                           _navigationService.GoBack();
                       }));
            }
        }

        public TaskSubitemViewModel(INavigationService navigationService, ITaskSubitemDataService taskSubitemDataService, ITaskItemDataService taskItemDataService, IUserDataService userDataService, IRoleTypeDataService roleTypeDataService)
        {
            _navigationService = navigationService;
            _taskSubitemDataService = taskSubitemDataService;
            _taskItemDataService = taskItemDataService;
            StartDateTime = EndDateTime = DateTime.Today;
            _userDataService = userDataService;
            _roleTypeDataService = roleTypeDataService;
        }

        private async void Initialize()
        {
            IsBusy = true;
            AvailableTaskStatuses = await _taskItemDataService.GetAvailableTaskStatuses();
            SelectedTaskStatus = AvailableTaskStatuses.First();
            if (_associatedTaskSubitem != null && _associatedTaskItem != null)
            {

                AvailableTaskSubitems = await _taskSubitemDataService.GetTaskSubitems(_associatedTaskItem.Id);
                Id = _associatedTaskSubitem.Id;
                Name = _associatedTaskSubitem.Name;
                Description = _associatedTaskSubitem.Description;
                SelectedTaskStatus = _availableTaskStatuses.Single(s => s.Id == _associatedTaskSubitem.TaskStatusId);
                StartDateTime = _associatedTaskSubitem.StartDateTime.Value;
                EndDateTime = _associatedTaskSubitem.EndDateTime.Value;
                EstimationInHours = _associatedTaskSubitem.EstimationInHours;
                IsCompleted = _associatedTaskSubitem.IsCompleted;
                IsDeleted = _associatedTaskSubitem.IsDeleted;
                SelectedPredecessor = !String.IsNullOrWhiteSpace(_associatedTaskSubitem.PredecessorId) ? AvailableTaskSubitems.SingleOrDefault(t => t.Id == _associatedTaskSubitem.PredecessorId) : null;
                SelectedSuccessor = !String.IsNullOrWhiteSpace(_associatedTaskSubitem.SuccessorId) ? AvailableTaskSubitems.SingleOrDefault(t => t.Id == _associatedTaskSubitem.SuccessorId) : null;
                AvailableGroupUsers = await _userDataService.GetUsersFromGroup(_associatedTaskItem.GroupId);
                SelectedExecutor = !String.IsNullOrWhiteSpace(_associatedTaskSubitem.ExecutorId) ? AvailableGroupUsers.SingleOrDefault(u => u.Id == _associatedTaskSubitem.ExecutorId) : null;
            }
            else if (_associatedTaskItem != null)
            {
                AvailableTaskSubitems = await _taskSubitemDataService.GetTaskSubitems(_associatedTaskItem.Id);
                AvailableGroupUsers = await _userDataService.GetUsersFromGroup(_associatedTaskItem.GroupId);
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
            Tuple<TaskItem, TaskSubitem> obj = parameter as Tuple<TaskItem, TaskSubitem>;
            if (obj != null)
            {
                _associatedTaskItem = obj.Item1;
                _associatedTaskSubitem = obj.Item2;
            }
            Initialize();
        }

        public void Deactivate(object parameter)
        {
            Id = String.Empty;
            Name = Description = String.Empty;
            EstimationInHours = 0;
            SelectedPredecessor = SelectedSuccessor = null;
            SelectedTaskStatus = null;
            SelectedExecutor = null;
        }
    }
}
