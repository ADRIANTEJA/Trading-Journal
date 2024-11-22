using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class LongToDateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return "--";

        var dateTicks = (long)value;

        if (dateTicks == 0) return "--";

        return new DateTime(dateTicks).ToString("dd/MM/yyyy hh.mm tt");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("No need to implement this for now");
    }
}
