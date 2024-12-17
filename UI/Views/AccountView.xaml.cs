using API.Events;
using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using MainModule.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UI.Common.Converters;
using UI.Common.Helpers;
using static MainModule.Common.Enums;

namespace UI.Views;
/// <summary>
/// Interaction logic for AccountView.xaml
/// </summary>
public partial class AccountView : UserControl
{
    private static Func<ChartPoint, string> _performanceLabelFormatter;

    private static List<Color> existingColors = [];

    private object dataContextRef;

    public AccountView()
    {
        InitializeComponent();

        _performanceLabelFormatter = (point) =>
        {
            switch (Tag)
            {
                case PerfomanceTimeFrame.Daily:
                    return Math.Round(point.Y, 2).ToString() + "% " + new DateTime((long)point.X).ToString(" MM/dd/yyyy");
                case PerfomanceTimeFrame.Monthly:
                    return Math.Round(point.Y, 2).ToString() + "% " + new DateTime((long)point.X).ToString(" MMMM yyyy");
                case PerfomanceTimeFrame.Yearly:
                    return Math.Round(point.Y, 2).ToString() + "% " + new DateTime((long)point.X).ToString(" yyyy");
            }
            throw new Exception("it should return one of the previous, else...something went really wrong");
        };
    }

    private void OnAccountViewLoadedHandler(object sender, RoutedEventArgs e)
    {
        dataContextRef = DataContext;

        var dataContext = (AccountViewModel)dataContextRef;
        dataContext.PerformanceViewModel.LoadDailyPerformanceCommand
            .Execute(dataContext.SelectedAccount.Id);

        SetBinding(TagProperty, new Binding("PerformanceViewModel.AccountPerformanceTimeFrame")
        {
            Mode = BindingMode.TwoWay
        });

        switch (Tag)
        {
            case PerfomanceTimeFrame.Daily:
                daily_performance_button.Background = ResourceAccessHelper.GreenBrushRef;
                break;
            case PerfomanceTimeFrame.Monthly:
                monthly_performance_button.Background = ResourceAccessHelper.GreenBrushRef;
                break;
            case PerfomanceTimeFrame.Yearly:
                yearly_performance_button.Background = ResourceAccessHelper.GreenBrushRef;
                break;
        }
    }

    private void ShowFilterPerformanceByDateOptionsHandler(object sender, MouseEventArgs e)
    {
        var dataContext = (AccountViewModel)dataContextRef;
        var sBoard = (Storyboard)Resources["expand_filter_performance_by_date_control_storyboard"];

        if (dataContext.PerformanceViewModel.AccountPerformance.Count > 0) sBoard.Begin();
    }

    private void HideFilterPerformanceByDateOptionsHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["collapse_filter_performance_by_date_control_storyboard"];
        sBoard.Begin();
    }

    private void ShowROIFormatOptionsHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["expand_roi_format_options_storyboard"];
        sBoard.Begin();
    }

    private void HideROIFormatOptionsHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["collapse_roi_format_options_storyboard"];
        sBoard.Begin();
    }

    private void HoldTimeFrameButtonHighlightHandler(object sender, RoutedEventArgs e)
    {
        var senderButtonRef = (Button)sender;

        daily_performance_button.Background = null;
        monthly_performance_button.Background = null;
        yearly_performance_button.Background = null;

        senderButtonRef.Background = ResourceAccessHelper.GreenBrushRef;

        PerformanceTimeFrameChangedHandler(senderButtonRef.Name);
    }

    private void PerformanceTimeFrameChangedHandler(string timeFramePressedButtonName)
    {
        var dataContext = (AccountViewModel)dataContextRef;

        switch (timeFramePressedButtonName)
        {
            case "daily_performance_button":
                Tag = PerfomanceTimeFrame.Daily;
                break;
            case "monthly_performance_button":
                Tag = PerfomanceTimeFrame.Monthly;
                break;
            case "yearly_performance_button":
                Tag = PerfomanceTimeFrame.Yearly;
                break;
        }

        dataContext.PerformanceViewModel.LoadDailyPerformanceCommand.Execute(dataContext.SelectedAccount.Id);
    }

    private void OnAccountPerformanceChartLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)dataContextRef;
        var performanceLineSeries = (LineSeries)account_performance_line_chart.Series[0];
        performanceLineSeries.LabelPoint = _performanceLabelFormatter;
       
        dataContext.PerformanceViewModel.AccountPerformance.CollectionChanged += OnAccountPerformanceChangedHandler;

        //var chartAxis = (Axis)account_performance_line_chart.FindName("x_axis");
        //if (dataContext.PerformanceViewModel.AccountPerformance.Any())
        //{
        //    chartAxis.MaxValue = dataContext.PerformanceViewModel.AccountPerformance.Max(point => point.X);
        //}

        performanceLineSeries.SetBinding(LineSeries.ValuesProperty, "PerformanceViewModel.AccountPerformance");

        //var maxValueBinding = new Binding("PerformanceViewModel.AccountPerformance")
        //{
        //    Mode = BindingMode.OneWay,
        //    Converter = new CollectionToLastValueConverter()
        //};

        //chartAxis.SetBinding(Axis.MaxRangeProperty, maxValueBinding);

        
        //chartAxis.MaxValue = 662688000000000000;
    }

    private void OnAccountPerformanceChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        ResetPerformanceXAxisView();

        //var dataContext = (AccountViewModel)dataContextRef;
        //var chartAxis = (Axis)account_performance_line_chart.FindName("x_axis");

        //if (dataContext.PerformanceViewModel.AccountPerformance.Any() && e.Action == NotifyCollectionChangedAction.Add)
        //{
        //    chartAxis.MaxValue = dataContext.PerformanceViewModel.AccountPerformance.Max(point => point.X +1);
        //}
    }

    private void OnFilterPerformanceControlLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)dataContextRef;
        options_list_view.SetBinding(ListView.ItemsSourceProperty, "PerformanceViewModel.AccountPerformance");
    }

    private void OnSelectedDateFilterHandler(object sender, MouseButtonEventArgs e)
    {
        var senderRef = (TextBlock)sender;

        var dataContext = (AccountViewModel)dataContextRef;
        dataContext.PerformanceViewModel.FilterAccountPerformanceByDateCommand.Execute((long)(double)senderRef.Tag);
    }

    private void OnStrategyUsageSeriesChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            strategy_use_pie_chart.SeriesColors.Add(GenerateRandomColor(new(), 0, 100, 115, 215, 92, 192));
    }

    private void OnStrategyUsePieChartLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)dataContextRef;
        dataContext.StrategyUsageSeries.CollectionChanged += OnStrategyUsageSeriesChangedHandler;

        var baseColor = Color.FromRgb(0, 115, 92);
        existingColors.Add(baseColor);
        strategy_use_pie_chart.SeriesColors = [baseColor];

        dataContext.EventAggregator.GetEvent<StrategyDataRquiredIntermediaryEvent>().Publish();
    }

    public Color GenerateRandomColor(Random rand,
                                     int minRVal,
                                     int maxRVal,
                                     int minGVal,
                                     int maxGVal,
                                     int minBVal,
                                     int maxBVal)
    {
        Color color;

        do
        {
            byte r = (byte)rand.Next(minRVal, maxRVal);
            byte g = (byte)rand.Next(minGVal, maxGVal);
            byte b = (byte)rand.Next(minBVal, maxBVal);
            color = Color.FromRgb(r, g, b);
        }
        while (!IsColorUnique(color, existingColors));

        existingColors.Add(color); return color;
    }

    private bool IsColorUnique(Color color, List<Color> existingColors)
    {
        int minRange = 5;

        foreach (var existingColor in existingColors)
        {
            if (CalculateColorDistance(color, existingColor) < minRange) return false;
        }
        return true;
    }

    private double CalculateColorDistance(Color color1, Color color2)
    {
        int rDifference = color1.R - color2.R; 
        int gDifference = color1.G - color2.G; 
        int bDifference = color1.B - color2.B;

        return Math.Sqrt(rDifference * rDifference + gDifference * gDifference + bDifference * bDifference);
    }

    private void OnStrategyUsePieChartUnloadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)dataContextRef;
        dataContext.StrategyUsageSeries.CollectionChanged -= OnStrategyUsageSeriesChangedHandler;
        dataContext.PerformanceViewModel.AccountPerformance.CollectionChanged -= OnAccountPerformanceChangedHandler;
    }

    private void ResetPerformanceChartButtonHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)dataContextRef;
        dataContext.PerformanceViewModel.
            LoadDailyPerformanceCommand.Execute(dataContext.SelectedAccount.Id);

        ResetPerformanceXAxisView();
    }

    private void ResetPerformanceXAxisView()
    {
        var chartXAxis = (Axis)account_performance_line_chart.FindName("x_axis");
        chartXAxis.MinValue = double.NaN;
        chartXAxis.MaxValue = double.NaN;
        chartXAxis.MinValue = double.NaN;
        chartXAxis.MaxValue = double.NaN;
    }
}
