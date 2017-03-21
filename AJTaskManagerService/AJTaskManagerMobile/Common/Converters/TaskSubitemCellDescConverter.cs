using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.Common.Converters
{
    public class TaskSubitemCellDescConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var appointments = parameter as Dictionary<DateTime, TaskSubitem>;
            if (value is DateTime)
            {
                var date = (DateTime)value;
                if (appointments != null)
                {
                    if (appointments.ContainsKey(date))
                    {
                        return appointments[date].Description;
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
