using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using System.Windows;
using System.Windows.Media;
using UI.Common.Helpers;

namespace UI.Common.Resources.Dictionaries;

public partial class MainResourceDictionary : ResourceDictionary
{
    private static readonly Func<double, string> roiValueLabelFormatter = value =>
    {
        if (value != 0)
        {
            string stringValue = Math.Round(value, 0).ToString();
            return string.Concat(stringValue.AsSpan(0, stringValue.Length - 3), " K");
        }

        return 0.ToString();
    };

    public static Func<ChartPoint, string> strategyPerformanceLabelFormater = chartPoint => 
        string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

    private static readonly CartesianMapper<ObservablePoint> performanceLineSeriesMapper = Mappers.Xy<ObservablePoint>()
      .X(point => point.X)
      .Y(point => point.Y)
      .Stroke(point => point.Y < 0 ? ResourceAccessHelper.SalmonBrushRef : ResourceAccessHelper.GreenBrushRef)
      .Fill(point => point.Y < 0 ? ResourceAccessHelper.SalmonBrushRef : ResourceAccessHelper.GreenBrushRef);

    public MainResourceDictionary()
    {
        Add("performance_line_series_mapper", performanceLineSeriesMapper);
        Add("roi_value_label_formatter", roiValueLabelFormatter);
        Add("strategy_performance_label_formater", strategyPerformanceLabelFormater);
    }
}
