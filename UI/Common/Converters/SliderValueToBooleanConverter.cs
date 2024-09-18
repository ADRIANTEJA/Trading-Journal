using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class SliderValueToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var doubleValue = (double)value;
        var doubleParameter = (string)parameter;

        if (doubleValue >= Double.Parse(doubleParameter)) return true;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
