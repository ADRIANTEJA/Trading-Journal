using System.Globalization;
using System.Windows.Data;

namespace MainModule.Common.Converters;

public class LongToDateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateInSeconds = (long)value;

        //find out how to convert the date to be used in the performance chart

        var convertedDate = new DateTime(dateInSeconds);

        return convertedDate.ToString("dd/MM/yyyy");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
