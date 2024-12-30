using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class ZeroToNoValueRepresentationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((double?)value == 0 || value == null) return "--";
        else return Math.Round((double)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Cannnot convert back");
    }
}
