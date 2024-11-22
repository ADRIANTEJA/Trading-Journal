using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class ZeroToOneValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value == 0) return 1;
        else return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Not supported");
    }
}
