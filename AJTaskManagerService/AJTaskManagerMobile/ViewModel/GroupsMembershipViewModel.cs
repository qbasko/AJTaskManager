using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace AJTaskManagerMobile.ViewModel
{
    public class GroupsMembershipViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly IUserGroupDataService _userGroupDataService;
        private readonly IGroupDataService _groupDataService;
        private readonly IUserDataService _userDataService;
        private RelayCommand _deleteSelectedCommand;

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

        public RelayCommand DeleteSelectedCommand
        {
            get
            {
                return _deleteSelectedCommand ?? (
                    _deleteSelectedCommand = new RelayCommand(async () =>
                    {
                        IsBusy = true;
                        var currentUser = await _userDataService.GetUser(AccountHelper.GetCurrentUserId(), UserDomainsEnum.Microsoft);
                        var selectedGroups = UserGroups.Where(ug => ug.IsChecked);
                        if (selectedGroups.Any(g => g.GroupName.Contains(Constants.DefaultGroupForUserNamePrefix)))
                            new MessageDialog(Constants.DefaultGroupCantBeDeleted).ShowAsync();
                        else
                        {
                            foreach (var selectedGroup in selectedGroups)
                            {
                                var userGroups = await _userGroupDataService.GetUserGroupTableForGroup(selectedGroup.Id);
                                var userGroup = userGroups.Single(ug => ug.UserId == currentUser.Id);
                                await _userGroupDataService.DeleteUserGroup(userGroup);
                            }
                            Refresh();
                        }
                        IsBusy = false;
                    }));
            }
        }

        private ObservableCollection<Group> _userGroups;
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

        public GroupsMembershipViewModel(INavigationService navigationService, IUserGroupDataService userGroupDataService, IUserDataService userDataService, IGroupDataService groupDataService)
        {
            _navigationService = navigationService;
            _userGroupDataService = userGroupDataService;
            _userDataService = userDataService;
            _groupDataService = groupDataService;
        }

        private async void Refresh()
        {
            IsBusy = true;
            UserGroups = await _groupDataService.GetGroupsAvailableForUser(AccountHelper.GetCurrentUserId());
            IsBusy = false;
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
