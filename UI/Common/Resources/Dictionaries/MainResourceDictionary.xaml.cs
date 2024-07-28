using LiveCharts;
using System.Windows;

namespace UI.Common.Resources.Dictionaries;

public partial class MainResourceDictionary : ResourceDictionary
{
    private static readonly Func<ChartPoint, string> dailyPerformanceLabelFormatter = 
        (point) => new DateTime((long)point.X).ToString("yyyy-MM-dd");

    private static readonly Func<double, string> roiValueLabelFormatter = 
        (value) =>
        {
            if (value != 0) return Math.Round(value, 0) + " K";
            return 0.ToString();
        };

    public MainResourceDictionary()
    {
        Add("daily_performance_label_formatter", dailyPerformanceLabelFormatter);
        Add("roi_value_label_formatter", roiValueLabelFormatter);
    }
}
