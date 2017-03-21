using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace AJTaskManagerMobile.ViewModel
{
    public class PersonalSettingsViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly IUserDataService _userDataService;
        private User _currentUser;

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

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                    return;
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                    return;
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (_emailAddress == value)
                    return;
                _emailAddress = value;
                RaisePropertyChanged(() => EmailAddress);
            }
        }

        private RelayCommand _updateCommand;

        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand ??
                       (_updateCommand = new RelayCommand(async () =>
                       {
                           IsBusy = true;
                           _currentUser.UserName = FirstName;
                           _currentUser.LastName = LastName;
                           _currentUser.Email = EmailAddress;
                           await _userDataService.Update(_currentUser);
                           IsBusy = false;
                           _navigationService.GoBack();
                       }));
            }
        }

        private RelayCommand _cancelCommand;

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

        public PersonalSettingsViewModel(INavigationService navigationService, IUserDataService userDataService)
        {
            _navigationService = navigationService;
            _userDataService = userDataService;
            GetCurrentUser();
        }

        private async void GetCurrentUser()
        {
            IsBusy = true;
            _currentUser = await _userDataService.GetUser(Helpers.AccountHelper.GetCurrentUserId(), UserDomainsEnum.Microsoft);
            FirstName = _currentUser.UserName;
            LastName = _currentUser.LastName;
            EmailAddress = _currentUser.Email;
            IsBusy = false;
        }

        public void Activate(object parameter)
        {
            
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
