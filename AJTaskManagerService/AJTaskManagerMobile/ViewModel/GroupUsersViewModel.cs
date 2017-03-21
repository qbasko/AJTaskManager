using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Views.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.ViewModel
{
    public class GroupUsersViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly IUserDataService _userDataService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private readonly IUserGroupDataService _userGroupDataService;
        private Group _associatedGroup;
        private string _groupTitle;
        private bool _isBusy;
        private ObservableCollection<User> _users;
        private ObservableCollection<RoleType> _availableRoleTypes;
        private RoleType _selectedRoleType;
        private RelayCommand _openAddItemPopupCommand;
        private RelayCommand<object> _addNewItemCommand;
        private RelayCommand _deleteSelectedCommand;
        private RelayCommand<object> _navigateToUsersPermissionsCommand;
        private RelayCommand<object> _selectedGroupCommand;


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

        public string GroupTitle
        {
            get { return _groupTitle; }
            set
            {
                if (_groupTitle == value)
                    return;
                _groupTitle = value;
                RaisePropertyChanged(() => GroupTitle);
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                if (_users == value)
                    return;
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }

        public ObservableCollection<RoleType> AvailableRoleTypes
        {
            get { return _availableRoleTypes; }
            set
            {
                if (_availableRoleTypes == value)
                    return;
                _availableRoleTypes = value;
                RaisePropertyChanged(() => AvailableRoleTypes);
            }
        }

        public RoleType SelectedRoleType
        {
            get { return _selectedRoleType; }
            set
            {
                if (_selectedRoleType == value)
                    return;
                _selectedRoleType = value;
                RaisePropertyChanged(() => SelectedRoleType);
            }
        }

        public RelayCommand OpenAddItemPopupCommand
        {
            get
            {
                return _openAddItemPopupCommand ??
                       (_openAddItemPopupCommand = new RelayCommand(() =>
                       {
                           Popup popup = new Popup();
                           var addUser = new AddGroupUserControl()
                            {
                                DataContext = this,
                                Width = 400,
                                Height = 240,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness() { Top = 30 }
                            };
                           popup.Child = addUser;
                           popup.IsLightDismissEnabled = true;
                           popup.IsOpen = true;
                       }));
            }
        }

        public RelayCommand<object> AddNewItemCommand
        {
            get
            {
                return _addNewItemCommand ??
                       (_addNewItemCommand = new RelayCommand<object>(async (obj) =>
                       {
                           IsBusy = true;
                           TextBox txtBox = obj as TextBox;
                           if (txtBox != null)
                           {
                               string userEmail = txtBox.Text;
                               var user = await _userDataService.GetUsersByEmail(userEmail);
                               if (user != null)
                               {
                                   UserGroup userGroup = new UserGroup();
                                   userGroup.Id = Guid.NewGuid().ToString();
                                   userGroup.GroupId = _associatedGroup.Id;
                                   userGroup.IsUserDefaultGroup = false;
                                   userGroup.RoleTypeId = SelectedRoleType.Id;
                                   userGroup.UserId = user.Id;
                                   var userGroups = await _userGroupDataService.GetUserGroupTableForGroup(_associatedGroup.Id);
                                   if (userGroups.All(ug => ug.UserId != user.Id))
                                   {
                                       await _userGroupDataService.Insert(userGroup);
                                       Refresh();
                                   }
                                   else
                                   {
                                       await new MessageDialog(Constants.UserWithEmailAlreadyExists).ShowAsync();
                                   }
                               }
                               else
                               {
                                   await new MessageDialog(Constants.UserWithEmailDoesntExist).ShowAsync();
                               }
                           }
                           IsBusy = false;
                       }));
            }
        }

        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return _deleteSelectedCommand ?? (_deleteSelectedCommand = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    var selectedUsers = Users.Where(u => u.IsChecked);
                    var userGroups = await _userGroupDataService.GetUserGroupTableForGroup(_associatedGroup.Id);
                    foreach (var selectedUser in selectedUsers)
                    {
                        UserGroup userGroup = userGroups.Single(ug => ug.UserId == selectedUser.Id);
                        await _userGroupDataService.DeleteUserGroup(userGroup);
                    }
                    Refresh();
                }));
            }
        }

        public RelayCommand<object> NavigateToUsersPermissionsCommand
        {
            get
            {
                return _navigateToUsersPermissionsCommand ?? (
                    _navigateToUsersPermissionsCommand = new RelayCommand<object>((obj) =>
                    {
                        //Messenger.Default.Send(_associatedGroup);
                        Tuple<User, Group> param = new Tuple<User, Group>((User)obj, _associatedGroup);
                        _navigationService.NavigateTo(Constants.UsersPermissionsPageKey, param);
                    }));
            }
        }

        //public RelayCommand<object> SelectedGroupCommand
        //{
        //    get
        //    {
        //        return _selectedGroupCommand ??
        //               (_selectedGroupCommand = new RelayCommand<object>((obj) =>
        //               {
        //                   Messenger.Default.Send(_associatedGroup);
        //               }));
        //    }
        //}

        public GroupUsersViewModel(INavigationService navigationService, IUserDataService userDataService, IRoleTypeDataService roleTypeDataService, IUserGroupDataService userGroupDataService)
        {
            _navigationService = navigationService;
            _userDataService = userDataService;
            _roleTypeDataService = roleTypeDataService;
            _userGroupDataService = userGroupDataService;
            Initialize();
        }

        private async void Initialize()
        {
            IsBusy = true;
            AvailableRoleTypes = await _roleTypeDataService.GetAllRoleTypes();
            IsBusy = false;
        }

        public void Activate(object parameter)
        {
            _associatedGroup = parameter as Group;
            GroupTitle = _associatedGroup != null ? _associatedGroup.GroupNameTruncated : String.Empty;
            Refresh();
        }

        public void Deactivate(object parameter)
        {
            if (Users != null)
                Users.Clear();
        }

        private async void Refresh()
        {
            try
            {
                IsBusy = true;
                Users = await _userDataService.GetUsersFromGroup(_associatedGroup.Id);
            }
            catch (Exception ex)
            {

            }
            IsBusy = false;
        }
    }
}
