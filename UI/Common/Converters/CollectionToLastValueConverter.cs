using LiveCharts;
using LiveCharts.Defaults;
using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;
public class CollectionToLastValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var collection = (ChartValues<ObservablePoint>)value;

        if (collection.Any()) 
            return collection.Max(point => point.X +1000000);
        else return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("cannot convert back");
    }
}
