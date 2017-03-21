using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using AJTaskManagerMobile.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using AJTaskManagerMobile.Model;
using Microsoft.WindowsAzure.MobileServices;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Model.MainHub;
using Syncfusion.CompoundFile.XlsIO.Native;

using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Syncfusion.Data.Extensions;
using WinRTXamlToolkit.Common;

namespace AJTaskManagerMobile.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUserDataService _userDataService;
        private readonly INavigationService _navigationService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private string _clock = "Starting...";
        private RelayCommand _incrementCommand;
        private int _index;
        private RelayCommand<string> _navigateCommand;
        private RelayCommand<object> _navigateToDoListsCommand;
        private bool _isBusy;
        private bool _isNotificationCreatorEnabled;
        private RelayCommand<string> _showDialogCommand;
        private RelayCommand _loginCommand;
        private RelayCommand<SectionItem> _navigateToItemCommand;
        private RelayCommand _navigateToGroupsCommand;
        private RelayCommand _logoutCommand;
        private string _welcomeTitle = "Hello MVVM";
        private ObservableCollection<SectionItemsGroup> _groups;
        public ObservableCollection<SectionItemsGroup> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                RaisePropertyChanged(() => Groups);
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

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set
            {
                if (_welcomeMessage == value)
                    return;
                _welcomeMessage = value;
                RaisePropertyChanged(() => WelcomeMessage);
            }
        }

        #region SignInDialogProperties
        private string _signinMessage;
        public string SigninMessage
        {
            get { return _signinMessage; }
            set
            {
                if (_signinMessage == value)
                    return;
                _signinMessage = value;
                RaisePropertyChanged(() => SigninMessage);
            }
        }

        private string _userFirstName;
        public string UserFirstName
        {
            get { return _userFirstName; }
            set
            {
                if (_userFirstName == value)
                    return;
                _userFirstName = value;
                RaisePropertyChanged(() => UserFirstName);
            }
        }

        private string _userLastName;
        public string UserLastName
        {
            get { return _userLastName; }
            set
            {
                if (_userLastName == value)
                    return;
                _userLastName = value;
                RaisePropertyChanged(() => UserLastName);
            }
        }

        private string _userEmailAddress;
        public string UserEmailAddress
        {
            get { return _userEmailAddress; }
            set
            {
                if (_userEmailAddress == value)
                    return;
                _userEmailAddress = value;
                RaisePropertyChanged(() => UserEmailAddress);
            }
        }

        private RelayCommand _cancelSigninCommand;
        public RelayCommand CancelSigninCommand
        {
            get
            {
                return _cancelSigninCommand ??
                       (_cancelSigninCommand = new RelayCommand(() =>
                       {
                           IsLoginVisible = true;
                           RaisePropertyChanged(() => IsLoginVisible);
                           _navigationService.GoBack();
                       }));
                ;
            }
        }

        private RelayCommand<object> _signinCommand;
        public RelayCommand<object> SigninCommand
        {
            get
            {
                return _signinCommand ??
                       (_signinCommand = new RelayCommand<object>(async obj =>
                       {
                           IsBusy = true;
                           User user = new User()
                           {
                               Id = Guid.NewGuid().ToString(),
                               UserName = UserFirstName,
                               LastName = UserLastName,
                               Email = UserEmailAddress
                           };
                           if (!String.IsNullOrWhiteSpace(user.UserName) && !String.IsNullOrWhiteSpace(user.LastName) &&
                               !String.IsNullOrWhiteSpace(user.Email))
                           {
                               var externalUserId = AccountHelper.GetCurrentUserId();
                               await _userDataService.Insert(user, Constants.MainAuthenticationDomain, externalUserId);
                               IsUserDataDialogVisible = false;
                           }
                           IsBusy = false;
                       }));
            }
        }

        public RelayCommand NotificationCreatorCommand
        {
            get
            {
                return new RelayCommand(async () =>
                    await Task.Run(async () =>
                    {
                        while (_isNotificationCreatorEnabled)
                        {
                            var notifications = await GetNotifications();
                            if (notifications.Any())
                            {
                                notifications.ForEach(NotificationPresenter.ShowNotification);
                            }
                            await Task.Delay(TimeSpan.FromHours(12));
                        }
                    })
                );
            }
        }
        #endregion

        private bool _isLoginVisible = false;
        public bool IsLoginVisible
        {
            get { return _isLoginVisible; }
            set
            {
                if (value == _isLoginVisible)
                    return;
                _isLoginVisible = value;
                RaisePropertyChanged(() => IsLoginVisible);
                RaisePropertyChanged(() => IsUserDataDialogVisible);
                RaisePropertyChanged(() => IsStartVisible);
            }
        }

        private bool _isUserDataDialogVisible;
        public bool IsUserDataDialogVisible
        {
            get { return _isUserDataDialogVisible; }
            set
            {
                if (_isUserDataDialogVisible == value)
                    return;
                _isUserDataDialogVisible = value;
                RaisePropertyChanged(() => IsUserDataDialogVisible);
                RaisePropertyChanged(() => IsStartVisible);
            }
        }

        //private bool _isStartVisible;

        public bool IsStartVisible
        {
            get
            {
                return !_isLoginVisible && !_isUserDataDialogVisible;
            }
        }

        public RelayCommand<object> NavigateToDoListsCommand
        {
            get
            {
                return _navigateToDoListsCommand ??
                       (_navigateToDoListsCommand = new RelayCommand<object>(
                           (obj) => _navigationService.NavigateTo(Constants.ToDoListsPageKey, null)));
                //(obj) => _navigationService.NavigateTo(GetMainSectionNavigationKey(obj), null)));
            }
        }

        public RelayCommand<SectionItem> NavigateToItemCommand
        {
            get
            {
                return _navigateToItemCommand ??
                    (_navigateToItemCommand = new RelayCommand<SectionItem>((item) =>
                    {
                        IsBusy = true;
                        if (item.UniqueId != Constants.LogoutActionKey)
                            _navigationService.NavigateTo(item.UniqueId, item);
                        else
                        {
                            LogoutCommand.Execute(this);
                        }
                        IsBusy = false;
                    }))
                ;
            }
        }

        public RelayCommand NavigateToGroupsCommand
        {
            get
            {
                return _navigateToGroupsCommand ??
                    (_navigateToGroupsCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo(Constants.GroupsPageKey, null);
                    }));
            }
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                    (_loginCommand = new RelayCommand(async () =>
                    {
                        IsBusy = true;
                        await DataServiceBase.Login();
                        if (DataServiceBase.HasUserLoggedIn())
                        {
                            IsLoginVisible = false;
                            UserFirstName = AccountHelper.GetUserFirstName();
                            UserLastName = AccountHelper.GetUserLastName();
                            UserEmailAddress = AccountHelper.GetUserEmail();
                            string userId = await _userDataService.GetUserInternalId(AccountHelper.GetCurrentUserId(),
                                UserDomainsEnum.Microsoft);
                            IsUserDataDialogVisible = String.IsNullOrWhiteSpace(userId);
                            SetWelcomeMessage();
                            RaisePropertyChanged(() => IsUserDataDialogVisible);
                        }
                        IsBusy = false;
                    }));
                ;
            }
        }

        public RelayCommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??
                       (_logoutCommand = new RelayCommand(() =>
                       {
                           DataServiceBase.Logout();
                           RaisePropertyChanged(() => Groups);
                           IsLoginVisible = true;
                           this.Cleanup();
                       }));
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(
            IUserDataService userDataService,
            INavigationService navigationService,
            ITaskSubitemDataService taskSubitemDataService)
        {
            _userDataService = userDataService;
            _navigationService = navigationService;
            _taskSubitemDataService = taskSubitemDataService;

            IsLoginVisible = !DataServiceBase.HasUserLoggedIn();
            SetWelcomeMessage();
            //IsUserDataDialogVisible = true;
            //IsStartVisible = !IsLoginVisible;
            MainHubDataModel.GetGroupsAsync().ContinueWith((task) =>
            {
                    Groups = new ObservableCollection<SectionItemsGroup>(task.Result);
            });
            _isNotificationCreatorEnabled = true;
            
            //_userDataService.Test();
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}

        private string GetMainSectionNavigationKey(object obj)
        {
            SectionItemsGroup sig = obj as SectionItemsGroup;
            if (sig == null) return String.Empty;
            return sig.UniqueId;
        }

        private async void SetWelcomeMessage()
        {
            if (!IsLoginVisible)
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(UserFirstName) || String.IsNullOrWhiteSpace(UserLastName) ||
                        String.IsNullOrWhiteSpace(UserEmailAddress))
                    {
                        var userId =
                            await
                                _userDataService.GetUserInternalId(AccountHelper.GetCurrentUserId(),
                                    Constants.MainAuthenticationDomain);
                        var user = await _userDataService.GetUserById(userId);
                        UserFirstName = user.UserName;
                        UserLastName = user.LastName;
                        UserEmailAddress = user.Email;
                    }
                    WelcomeMessage = String.Format(Constants.WelcomeMessage, UserFirstName, UserLastName,
                        UserEmailAddress);
                }
                catch (Exception ex)
                {
                    // ignored
                }
                NotificationCreatorCommand.Execute(null);
            }
        }

        private async Task<IEnumerable<Notification>> GetNotifications()
        {
            try
            {
                List<Notification> notifications = new List<Notification>();
                string userId = AccountHelper.GetCurrentUserId();
                string userInternalId =
                    await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
                var taskSubitemsAlreadyStarted =
                    await _taskSubitemDataService.GetUserTaskSubitemsAlreadyStarted(userInternalId);
                var taskSubitemsNearDeadlines =
                    await _taskSubitemDataService.GetUserTaskSubitemsNearDeadlines(userInternalId);
                foreach (var taskSubitem in taskSubitemsAlreadyStarted)
                {
                    Notification notification = new Notification();
                    notification.Title = String.Format(Constants.TaskSubitemAlreadyStartedTitle, taskSubitem.Name);
                    notification.Description = String.Format(Constants.TaskSubitemAlreadyStartedDesc,
                        taskSubitem.StartDateTime.Value.ToShortDateString());
                    notifications.Add(notification);
                }
                foreach (var taskSubitem in taskSubitemsNearDeadlines)
                {
                    Notification notification = new Notification();
                    notification.Title = String.Format(Constants.TaskSubitemNearDeadlineTitle, taskSubitem.Name);
                    notification.Description = String.Format(Constants.TaskSubitemNearDeadlineDesc,
                        taskSubitem.EndDateTime.Value.ToShortDateString());
                    notifications.Add(notification);
                }

                return notifications;
            }
            catch (Exception ex)
            {
                //ignore
            }
            return new List<Notification>();
        }
    }
}