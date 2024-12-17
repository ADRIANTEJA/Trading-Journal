using API.Events;
using LiveCharts;
using LiveCharts.Wpf;
using MainModule.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static MainModule.Common.Enums;

namespace UI.Views;
/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    private static Func<ChartPoint, string> _performanceLabelFormatter;

    private static List<Color> existingColors = [];

    private object dataContextRef;

    public HomeView()
    {
        InitializeComponent();

        _performanceLabelFormatter = (point) =>
        {
            switch (Tag)
            {
                case PerfomanceTimeFrame.Daily:
                    return Math.Round(point.Y, 2).ToString() + "% " + new DateTime((long)point.X).ToString(" MM/dd/yyyy");
                case PerfomanceTimeFrame.Monthly:
                    return Math.Round(point.Y, 2).ToString() + "% " + new DateTime((long)point.X).ToString(" MM/yyyy");
                case PerfomanceTimeFrame.Yearly:
                    return Math.Round(point.Y, 2).ToString() + "% " + new DateTime((long)point.X).ToString(" yyyy");
            }
            throw new Exception("it should return one of the previous, else...something went really wrong");
        };
    }

    private void OnHomeViewLoadedHandler(object sender, RoutedEventArgs e)
    {
        dataContextRef = DataContext;

        var dataContext = (HomeViewModel)DataContext;
        dataContext.AccountViewModel.LoadAccountsCommand.Execute(null);
        dataContext.LoadTradesCommand.Execute(null);
        dataContext.SymbolViewModel.LoadSymbolsCommand.Execute(null);
        dataContext.StrategyViewModel.LoadStrategiesCommand.Execute(null);
        dataContext.PerformanceViewModel.LoadDailyPerformanceCommand
            .Execute(dataContext.AccountViewModel.SelectedAccount.Id);

        SetBinding(TagProperty, new Binding("PerformanceViewModel.AccountPerformanceTimeFrame")
        {
            Mode = BindingMode.OneWay
        });
    }

    private void ShowSymbolCategoryHandler(object sender, RoutedEventArgs e)
    {
        if (expand_symbol_category_button.Tag.ToString() == "1")
        {
            expand_symbol_category_button.SetResourceReference(StyleProperty, "symbol_category_button_collapse_style");
            expand_symbol_category_button.Tag = "0";
            var sBoard = (Storyboard)Resources["show_symbol_categories_storyboard"];
            sBoard.Begin();
        }
        else
        {
            expand_symbol_category_button.SetResourceReference(StyleProperty, "symbol_category_button_expand_style");
            expand_symbol_category_button.Tag = "1";
            var sBoard = (Storyboard)Resources["hide_symbol_categories_storyboard"];
            sBoard.Begin();
        }
    }

    private void ShowSymbolCategoryFiltersHandller(object sender, MouseEventArgs e)
    {
        var senderRef = (Border)sender;

        Storyboard sBoard = new();

        switch(senderRef.Name)
        {
            case "crypto_expander":
                sBoard = (Storyboard)Resources["expand_crypto_filter_storyboard"];
                sBoard.Begin();
                break;
            case "forex_expander":
                sBoard = (Storyboard)Resources["expand_forex_filter_storyboard"];
                sBoard.Begin();
                break;
            case "indices_expander":
                sBoard = (Storyboard)Resources["expand_indices_filter_storyboard"];
                sBoard.Begin();
                break;
            case "etfs_expander":
                sBoard = (Storyboard)Resources["expand_etfs_filter_storyboard"];
                sBoard.Begin();
                break;
            case "stocks_expander":
                sBoard = (Storyboard)Resources["expand_stocks_filter_storyboard"];
                sBoard.Begin();
                break;
            case "commodities_expander":
                sBoard = (Storyboard)Resources["expand_commodities_filter_storyboard"];
                sBoard.Begin();
                break;
        }
    }

    private void HideSymbolCategoryFiltersHandller(object sender, MouseEventArgs e)
    {
        var senderRef = (Border)sender;

        Storyboard sBoard = new();

        switch (senderRef.Name)
        {
            case "crypto_expander":
                sBoard = (Storyboard)Resources["collapse_crypto_filter_storyboard"];
                sBoard.Begin();
                break;
            case "forex_expander":
                sBoard = (Storyboard)Resources["collapse_forex_filter_storyboard"];
                sBoard.Begin();
                break;
            case "indices_expander":
                sBoard = (Storyboard)Resources["collapse_indices_filter_storyboard"];
                sBoard.Begin();
                break;
            case "etfs_expander":
                sBoard = (Storyboard)Resources["collapse_etfs_filter_storyboard"];
                sBoard.Begin();
                break;
            case "stocks_expander":
                sBoard = (Storyboard)Resources["collapse_stocks_filter_storyboard"];
                sBoard.Begin();
                break;
            case "commodities_expander":
                sBoard = (Storyboard)Resources["collapse_commodities_filter_storyboard"];
                sBoard.Begin();
                break;
        }
    }

    private void OnAccountPerformanceChartLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (HomeViewModel)dataContextRef;
        var performanceLineSeries = (LineSeries)account_performance_line_chart.Series[0];
        performanceLineSeries.LabelPoint = _performanceLabelFormatter;

        dataContext.PerformanceViewModel.AccountPerformance.CollectionChanged += OnAccountPerformanceChangedHandler;
    }

    private void OnStrategyUsageSeriesChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            strategy_use_pie_chart.SeriesColors.Add(GenerateRandomColor(new(), 0, 100, 115, 215, 92, 192));
    }

    private void OnAccountPerformanceChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        ResetPerformanceXAxisView();
    }

    private void OnStrategyUsePieChartLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (HomeViewModel)dataContextRef;
        dataContext.AccountViewModel.StrategyUsageSeries.CollectionChanged += OnStrategyUsageSeriesChangedHandler;

        var baseColor = Color.FromRgb(0, 115, 92);
        existingColors.Add(baseColor);
        strategy_use_pie_chart.SeriesColors = [baseColor];

        dataContext.EventAggregator.GetEvent<StrategyDataRquiredIntermediaryEvent>().Publish();
    }

    private void OnStrategyUsePieChartUnloadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (HomeViewModel)dataContextRef;
        dataContext.AccountViewModel.StrategyUsageSeries.CollectionChanged -= OnStrategyUsageSeriesChangedHandler;
        dataContext.PerformanceViewModel.AccountPerformance.CollectionChanged -= OnAccountPerformanceChangedHandler;
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

    private void ResetPerformanceXAxisView()
    {
        var chartXAxis = (Axis)account_performance_line_chart.FindName("x_axis");
        chartXAxis.MinValue = double.NaN;
        chartXAxis.MaxValue = double.NaN;
        chartXAxis.MinValue = double.NaN;
        chartXAxis.MaxValue = double.NaN;
    }
}
