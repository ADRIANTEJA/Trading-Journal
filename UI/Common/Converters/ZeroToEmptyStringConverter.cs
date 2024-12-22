using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class ZeroToEmptyStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((double)value == 0) return "";
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
