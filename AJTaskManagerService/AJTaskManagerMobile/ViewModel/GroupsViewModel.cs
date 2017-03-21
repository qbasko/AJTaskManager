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
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Views.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.ViewModel
{
    public class GroupsViewModel : ViewModelBase, INavigable
    {
        private readonly IGroupDataService _groupDataService;
        private readonly IUserDataService _userDataService;
        private readonly INavigationService _navigationService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private bool _isBusy;
        private string _userInternalId;
        private RelayCommand _openAddItemPopupCommand;
        private RelayCommand _deleteSelectedCommand;
        private RelayCommand<object> _addNewItemCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand<object> _navigateToGroupUsersCommand;
        private ObservableCollection<Group> _userGroups;


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

        public RelayCommand OpenAddItemPopupCommand
        {
            get
            {
                return _openAddItemPopupCommand ??
                       (_openAddItemPopupCommand = new RelayCommand(async () =>
                       {
                           Popup popup = new Popup();
                           var addGroup = new AddItemControl()
                           {
                               DataContext = this,
                               Width = 400,
                               Height = 100,
                               HorizontalAlignment = HorizontalAlignment.Center,
                               VerticalAlignment = VerticalAlignment.Center,
                               Margin = new Thickness() { Top = 30 }
                           };
                           popup.Child = addGroup;
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
                               var userId = AccountHelper.GetCurrentUserId();
                               var user =
                                   await _userDataService.GetUser(userId, Constants.MainAuthenticationDomain);
                               Group group = new Group()
                               {
                                   Id = Guid.NewGuid().ToString(),
                                   GroupName = txtBox.Text
                               };
                               await _groupDataService.AddUserGroup(group, user);
                               Refresh();
                           }
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

        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return _deleteSelectedCommand ?? (_deleteSelectedCommand = new RelayCommand(async () =>
                {
                    var userInternalId = await GetUserInternalId();
                    var selectedGroups = UserGroups.Where(ug => ug.IsChecked);
                    bool canUserDelete = false;
                    foreach (var group in selectedGroups)
                    {
                        bool result = (await _roleTypeDataService.CanUserAddOrDeleteItem(userInternalId, group.Id));
                        if (!result)
                            return;
                        canUserDelete = true;
                    }
                    if (selectedGroups.Any(g => g.GroupName.Contains(Constants.DefaultGroupForUserNamePrefix)))
                    {
                        IsBusy = false;
                        new MessageDialog(Constants.CantDeleteDefaultGroup).ShowAsync();
                    }
                    else if (canUserDelete)
                    {
                        await _groupDataService.DeleteGroups(selectedGroups);
                        Refresh();
                    }
                    else
                    {
                        IsBusy = false;
                        new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                    }
                }));
            }
        }

        public RelayCommand<object> NavigateToGroupUsersCommand
        {
            get
            {
                return _navigateToGroupUsersCommand ??
                       (_navigateToGroupUsersCommand = new RelayCommand<object>(async (obj) =>
                       {
                           Group group = obj as Group;
                           if (group != null)
                           {
                               var userInternalId = await GetUserInternalId();
                               bool canUserAdd =
                                   await _roleTypeDataService.CanUserAddOrDeleteItem(userInternalId, group.Id);
                               if (canUserAdd)
                                   _navigationService.NavigateTo(Constants.GroupUsersPageKey, obj);
                               else
                                   new MessageDialog(Constants.UserCantAddOrDelete).ShowAsync();
                           }
                       }));
            }
        }

        public ObservableCollection<Group> UserGroups
        {
            get { return _userGroups; }
            set
            {
                if (_userGroups == value)
                    return;
                _userGroups = value;
                RaisePropertyChanged(() => UserGroups);
            }
        }


        public GroupsViewModel(IGroupDataService groupDataService, IUserDataService userDataService, INavigationService navigationService, IRoleTypeDataService roleTypeDataService)
        {
            _groupDataService = groupDataService;
            _userDataService = userDataService;
            _navigationService = navigationService;
            _roleTypeDataService = roleTypeDataService;
            Refresh();
        }

        private async void Refresh()
        {
            try
            {
                IsBusy = true;
                var userId = AccountHelper.GetCurrentUserId();
                //string userInternalId = await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                UserGroups = await _groupDataService.GetGroupsAvailableForUser(userId);
                UserGroups = UserGroups.OrderBy(g => g.GroupNameTruncated).ToObservableCollection();
            }
            catch (Exception ex)
            {

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

        }

        public void Deactivate(object parameter)
        {

        }
    }
}
