using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model;
using GalaSoft.MvvmLight.Command;


namespace AJTaskManagerMobile.DataServices
{
    public class SettingsService : ISettingsService
    {
        public System.Collections.ObjectModel.ObservableCollection<Model.Settings> GetSettings()
        {
            return new ObservableCollection<Settings>()
            {
                new Settings(){ Name = Common.Constants.PersonalInformationPageTitle, NavigateToPageId = Common.Constants.PersonalSettingsPageKey},
                new Settings(){Name = Common.Constants.GroupsMembershipsPageTitle, NavigateToPageId = Common.Constants.GroupsMembershipPageKey}
            };
        }
    }
}
