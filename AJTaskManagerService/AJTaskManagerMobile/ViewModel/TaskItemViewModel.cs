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
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Syncfusion.XlsIO.Implementation.XmlSerialization;

namespace AJTaskManagerMobile.ViewModel
{
    public class TaskItemViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly IGroupDataService _groupDataService;
        private readonly ITaskItemDataService _taskItemDataService;
        private readonly IUserDataService _userDataService;
        private readonly IRoleTypeDataService _roleTypeDataService;

        private string _name;
        private string _description;
        private AJTaskManagerMobile.Model.DTO.TaskStatus _selectedTaskTaskStatus;
        private ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskStatus> _availableTaskStatuses;
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private bool _isDeleted;
        private bool _isCompleted;
        private Group _selectedGroup;
        private ObservableCollection<Group> _availableGroups;
        private bool _isSaveEnabled;
        private RelayCommand _saveTaskItemCommand;
        private RelayCommand _cancelTaskItemCommand;
        private TaskItem _associatedTaskItem;
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

        public string Id { get; set; }

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

        public AJTaskManagerMobile.Model.DTO.TaskStatus SelectedTaskStatus
        {
            get { return _selectedTaskTaskStatus; }
            set
            {
                if (_selectedTaskTaskStatus == value)
                    return;
                _selectedTaskTaskStatus = value;
                RaisePropertyChanged(() => SelectedTaskStatus);
            }
        }

        public ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskStatus> AvailableTaskStatuses
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

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup == value)
                    return;
                _selectedGroup = value;
                RaisePropertyChanged(() => SelectedGroup);
            }
        }

        public ObservableCollection<Group> AvailableGroups
        {
            get { return _availableGroups; }
            set
            {
                if (_availableGroups == value)
                    return;
                _availableGroups = value;
                RaisePropertyChanged(() => AvailableGroups);
            }
        }

        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set
            {
                if (_isSaveEnabled == value)
                    return;
                _isSaveEnabled = value;
                RaisePropertyChanged(() => IsSaveEnabled);
            }
        }

        public RelayCommand SaveTaskItemCommand
        {
            get
            {
                return _saveTaskItemCommand ??
                       (_saveTaskItemCommand = new RelayCommand(async () =>
                       {
                           IsBusy = true;
                           var internalUserId = await GetUserInternalId();
                           TaskItem taskItem = new TaskItem()
                           {
                               Id = Id,
                               Name = Name,
                               Description = Description,
                               TaskStatusId = SelectedTaskStatus.Id,
                               StartDateTime = StartDateTime,
                               EndDateTime = EndDateTime,
                               GroupId = SelectedGroup.Id,
                               IsCompleted = false,
                               IsDeleted = false
                           };
                           if (String.IsNullOrWhiteSpace(taskItem.Id))
                           {
                               bool canUserAdd = await _roleTypeDataService.CanUserAddOrDeleteItem(internalUserId, SelectedGroup.Id);
                               taskItem.Id = Guid.NewGuid().ToString();
                               if (canUserAdd)
                               {
                                   await _taskItemDataService.InsertTaskItem(taskItem, internalUserId);
                                   IsBusy = false;
                                   _navigationService.GoBack();
                               }
                               else
                                   new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                           }
                           else
                           {
                               bool canUserEdit = await _roleTypeDataService.CanUserEditItem(internalUserId, SelectedGroup.Id);
                               if (canUserEdit)
                               {
                                   await _taskItemDataService.UpdateTaskItem(taskItem);
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

        public RelayCommand CancelTaskItemCommand
        {
            get
            {
                return _cancelTaskItemCommand ??
                       (_cancelTaskItemCommand = new RelayCommand(() =>
                       {
                           _navigationService.GoBack();
                       }));
            }
        }

        public TaskItemViewModel(INavigationService navigationService, ITaskItemDataService taskItemDataService, IGroupDataService groupDataService, IUserDataService userDataService, IRoleTypeDataService roleTypeDataDataService)
        {
            _navigationService = navigationService;
            _groupDataService = groupDataService;
            _taskItemDataService = taskItemDataService;
            _userDataService = userDataService;
            _roleTypeDataService = roleTypeDataDataService;
            StartDateTime = EndDateTime = DateTime.Today;
            Initialize();
        }

        private async void Initialize()
        {
            IsBusy = true;
            AvailableGroups = await _groupDataService.GetGroupsAvailableForUser(AccountHelper.GetCurrentUserId());
            AvailableTaskStatuses = await _taskItemDataService.GetAvailableTaskStatuses();
            SelectedTaskStatus = AvailableTaskStatuses.First();
            SelectedGroup =
                AvailableGroups.SingleOrDefault(g => g.GroupName.Contains(Constants.DefaultGroupForUserNamePrefix));
            
            if (_associatedTaskItem != null)
            {
                Id = _associatedTaskItem.Id;
                Name = _associatedTaskItem.Name;
                Description = _associatedTaskItem.Description;
                SelectedTaskStatus = _availableTaskStatuses.Single(s => s.Id == _associatedTaskItem.TaskStatusId);
                SelectedGroup = _availableGroups.Single(g => g.Id == _associatedTaskItem.GroupId);
                StartDateTime = _associatedTaskItem.StartDateTime.Value;
                EndDateTime = _associatedTaskItem.EndDateTime.Value;
                IsCompleted = _associatedTaskItem.IsCompleted;
                IsDeleted = _associatedTaskItem.IsDeleted;
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
            Initialize();
            _associatedTaskItem = parameter as TaskItem;

        }

        public void Deactivate(object parameter)
        {
            Id = String.Empty;
            Name = String.Empty;
            Description = String.Empty;
            SelectedGroup = null;
            SelectedTaskStatus = null;
            StartDateTime = EndDateTime = DateTime.Now;
        }
    }
}
