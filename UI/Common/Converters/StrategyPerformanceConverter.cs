using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MainModule.DataModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UI.Common.Helpers;

namespace UI.Common.Converters;

public class StrategyPerformanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var strategy = (Strategy)value;
        SeriesCollection performanceSeries;

        if (strategy != null)
        {
            var winPieSeries = new PieSeries
            {
                Values = new ChartValues<ObservableValue> { new(strategy.Wins) },
                Title = "Wins",
                LabelPoint = (Func<ChartPoint, string>)ResourceAccessHelper.StrategyPerformanceLabelFormaterRef,
                DataLabels = true,
                FontSize = ResourceAccessHelper.FontSize,
                Fill = ResourceAccessHelper.GreenBrushRef,
                Stroke = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) },
                StrokeThickness = 1.5,
            };
            var lossPieSeries = new PieSeries
            {
                Values = new ChartValues<ObservableValue>() { new(strategy.Losses) },
                Title = "Losses",
                LabelPoint = (Func<ChartPoint, string>)ResourceAccessHelper.StrategyPerformanceLabelFormaterRef,
                DataLabels = true,
                FontSize = ResourceAccessHelper.FontSize,
                Fill = ResourceAccessHelper.SalmonBrushRef,
                Stroke = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) },
                StrokeThickness = 1.5,
            };

            performanceSeries = [winPieSeries, lossPieSeries];
            return performanceSeries;
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
