using System;
using System.Globalization;
using Xamarin.Forms;

namespace OouiSignalRSample.Converters
{
    public class ActiveChatUserColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;
            return (bool)value ? Color.FromHex("#ededea") : Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
