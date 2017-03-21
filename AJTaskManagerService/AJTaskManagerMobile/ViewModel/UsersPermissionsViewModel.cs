using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.ViewModel
{
    public class UsersPermissionsViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly IUserGroupDataService _userGroupDataService;
        private readonly IRoleTypeDataService _roleTypeDataService;
        private User _associatedUser;
        private Group _associatedGroup;
        private string _usersEmail;
        private ObservableCollection<RoleType> _userPermissions;
        private RelayCommand<object> _permissionChangeCommand;

        public string UsersEmail
        {
            get { return _usersEmail; }
            set
            {
                if (_usersEmail == value)
                    return;
                _usersEmail = value;
                RaisePropertyChanged(() => UsersEmail);
            }
        }

        public ObservableCollection<RoleType> UserPermissions
        {
            get { return _userPermissions; }
            set
            {
                if (_userPermissions == value)
                    return;
                _userPermissions = value;
                RaisePropertyChanged(() => UserPermissions);
            }
        }

        public RelayCommand<object> PermissionChangeCommand
        {
            get
            {
                return _permissionChangeCommand ??
                       (_permissionChangeCommand = new RelayCommand<object>(async (obj) =>
                       {
                           var checkedRoleType = UserPermissions.FirstOrDefault(p => p.IsChecked);
                           var userGroup =
                               await _userGroupDataService.GetUserGroup(_associatedUser.Id, _associatedGroup.Id);
                           if (checkedRoleType.Id != userGroup.RoleTypeId)
                           {
                               userGroup.RoleTypeId = checkedRoleType.Id;
                               await _userGroupDataService.Update(userGroup);
                              // Refresh();
                           }
                       }));
            }
        }

        public UsersPermissionsViewModel(INavigationService navigationService, IUserGroupDataService userGroupDataService, IRoleTypeDataService roleTypeDataService)
        {
            _navigationService = navigationService;
            _userGroupDataService = userGroupDataService;
            _roleTypeDataService = roleTypeDataService;
            //Messenger.Default.Register<Group>(this, HandleMessage);
            Refresh();
        }

        //private void HandleMessage(Group obj)
        //{
        //    _associatedGroup = obj;
        //}

        public void Activate(object parameter)
        {
            Tuple<User, Group> userGroupTuple = (Tuple<User, Group>)parameter;
            _associatedUser = userGroupTuple.Item1;
            _associatedGroup = userGroupTuple.Item2;
            UsersEmail = _associatedUser != null ? _associatedUser.Email : String.Empty;
            Refresh();
        }

        public void Deactivate(object parameter)
        {
        }

        private async void Refresh()
        {
            if (_associatedGroup != null && _associatedUser != null)
            {
                var userGroup = await _userGroupDataService.GetUserGroup(_associatedUser.Id, _associatedGroup.Id);
                var roleTypes = await _roleTypeDataService.GetAllRoleTypes();
                if (userGroup != null)
                    roleTypes.Single(r => r.Id == userGroup.RoleTypeId).IsChecked = true;
                UserPermissions = roleTypes.ToObservableCollection();
            }
        }
    }
}
