using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using static MainModule.Common.Enums;

namespace UI.Common.Converters;

public class PerformanceDateTimeFrameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateTicks = (long)(double)value;

        var parameterAsListView = (ListView)parameter;
        var accountViewTag = (PerfomanceTimeFrame)parameterAsListView.Tag;

        switch (accountViewTag)
        {
            case PerfomanceTimeFrame.Daily:
                return new DateTime(dateTicks).ToString(" MM/dd/yyyy");
            case PerfomanceTimeFrame.Monthly:
                return new DateTime(dateTicks).ToString(" MMMM yyyy");
            case PerfomanceTimeFrame.Yearly:
                return new DateTime(dateTicks).ToString(" yyyy");
        }
        throw new Exception("it should return one of the previous, if not check the parameter");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
