using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.Common.Converters
{
    public class TaskItemCellConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dates = parameter as Dictionary<DateTime, TaskItem>;
            if (value is DateTime)
            {
                var date = (DateTime) value;
                if (dates != null)
                {
                    if (dates.ContainsKey(date))
                    {
                        return dates[date].Name;
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
