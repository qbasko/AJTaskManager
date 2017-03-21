using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model;

namespace AJTaskManagerMobile.DataServices
{
    public interface ISettingsService
    {
        ObservableCollection<Settings> GetSettings();
    }
}
