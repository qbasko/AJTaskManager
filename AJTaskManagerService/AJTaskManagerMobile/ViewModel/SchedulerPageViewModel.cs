using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using AJTaskManagerMobile.Common;
using GalaSoft.MvvmLight;
using Syncfusion.UI.Xaml.Schedule;

namespace AJTaskManagerMobile.ViewModel
{
    public class SchedulerPageViewModel : ViewModelBase, INavigable
    {

        private ScheduleAppointmentCollection _scheduleItems;

        public ScheduleAppointmentCollection ScheduleItems
        {
            get { return _scheduleItems; }
            set
            {
                if (_scheduleItems == value)
                    return;
                _scheduleItems = value;
                RaisePropertyChanged(() => ScheduleItems);
            }
        }

        public void Activate(object parameter)
        {
            _scheduleItems = new ScheduleAppointmentCollection();
            ScheduleItems.Add(new ScheduleAppointment(){Notes = "test", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2)});
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
