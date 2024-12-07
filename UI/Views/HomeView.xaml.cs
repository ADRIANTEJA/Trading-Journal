using LiveCharts;
using LiveCharts.Wpf;
using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static MainModule.Common.Enums;

namespace UI.Views;
/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    private static Func<ChartPoint, string> _performanceLabelFormatter;

    public HomeView()
    {
        InitializeComponent();

        _performanceLabelFormatter = (point) =>
        {
            switch (Tag)
            {
                case PerfomanceTimeFrame.Daily:
                    return Math.Round(point.Y, 2).ToString() + " % " + new DateTime((long)point.X).ToString("- MM/dd/yyyy");
                case PerfomanceTimeFrame.Monthly:
                    return Math.Round(point.Y, 2).ToString() + " % " + new DateTime((long)point.X).ToString("- MM/yyyy");
                case PerfomanceTimeFrame.Yearly:
                    return Math.Round(point.Y, 2).ToString() + " % " + new DateTime((long)point.X).ToString("- yyyy");
            }
            throw new Exception("it should return one of the previous, else...something went really wrong");
        };
    }

    private void OnHomeViewLoadedHandler(object sender, RoutedEventArgs e)
    {
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
    // find a way of changing the line series label formatter
    private void OnAccountPerformanceChartLoaded(object sender, RoutedEventArgs e)
    {
        var performanceLineSeries = (LineSeries)account_performance_line_chart.Series[0];

        performanceLineSeries.LabelPoint = _performanceLabelFormatter;
    }
}
