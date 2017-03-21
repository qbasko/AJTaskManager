using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace AJTaskManagerMobile.ViewModel
{
    public class SettingsViewModel : ViewModelBase, INavigable
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private ObservableCollection<Settings> _settings;
        public ObservableCollection<Settings> Settings
        {
            get { return _settings; }
            set
            {
                if (_settings == value)
                    return;
                _settings = value;
                RaisePropertyChanged(() => Settings);
            }
        }

        private RelayCommand<object> _navigateToSettingsCommand;

        public RelayCommand<object> NavigateToSettingsCommand
        {
            get
            {
                return _navigateToSettingsCommand ??
                    (_navigateToSettingsCommand = new RelayCommand<object>(obj =>
                    {
                        var settingsItem = obj as Settings;
                        if (settingsItem != null)
                            _navigationService.NavigateTo(settingsItem.NavigateToPageId);
                    }));
            }
        }

        public SettingsViewModel(INavigationService navigationService, ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
        }

        public void Activate(object parameter)
        {
            Settings = _settingsService.GetSettings();
        }

        public void Deactivate(object parameter)
        {
           
        }
    }
}
