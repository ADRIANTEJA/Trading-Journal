using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class DatePickerDateToTicksConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new DateTime((long)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = (DateTime)value;
        return date.Ticks;
    }
}
