using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AJTaskManagerMobile.Common.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool invert = parameter != null && parameter is string && (parameter as string) == "invert" ? true : false;
            bool val = (bool)value;

            if (val ^ invert)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
