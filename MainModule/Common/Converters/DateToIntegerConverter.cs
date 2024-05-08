
using System.Globalization;
using System.Windows.Data;

namespace MainModule.Common.Converters;

public class DateToIntegerConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateInSeconds = (long)value;

        var convertedDate = new DateTime(dateInSeconds);

        return convertedDate.ToString("dd/MM/yyyy");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
