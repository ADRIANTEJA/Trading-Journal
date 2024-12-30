using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class DoubleToTruncatedDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null ? Math.Round((double)value, 4).ToString() : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
