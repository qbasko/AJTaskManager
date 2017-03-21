using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace AJTaskManagerMobile.ViewModel
{
    public class CalendarWithTaskItemsViewModel : ViewModelBase, INavigable
    {
        private INavigationService _navigationService;
        //private Dictionary<DateTime, TaskItem> _calendarTaskItems;
        //public Dictionary<DateTime, TaskItem> CalendarTaskItems
        //{
        //    get { return _calendarTaskItems; }
        //    set
        //    {
        //        if (_calendarTaskItems == value)
        //            return;
        //        _calendarTaskItems = value;
        //        RaisePropertyChanged(() => CalendarTaskItems);
        //    }
        //}

        public CalendarWithTaskItemsViewModel(INavigationService ns)
        {
            _navigationService = ns;
            //CalendarTaskItems = new Dictionary<DateTime, TaskItem>();
            //CalendarTaskItems.Add(DateTime.Now, new TaskItem() { Name = "TEST1", Description = "TEST DESCRIPTION" });
        }

        private CalendarTaskItem _calendarTaskItems;
        public CalendarTaskItem CalendarTaskItems
        {
            get { return _calendarTaskItems; }
            set
            {
                if (_calendarTaskItems == value)
                    return;
                _calendarTaskItems = value;
                RaisePropertyChanged(() => CalendarTaskItems);
            }
        }

        public void Activate(object parameter)
        {
            CalendarTaskItems = new CalendarTaskItem();
            //CalendarTaskItems.Add(DateTime.Now, new TaskItem() { Name = "TEST1", Description = "TEST DESCRIPTION" });
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
