using LiveCharts;
using LiveCharts.Wpf;
using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UI.Common.Helpers;
using static MainModule.Common.Enums;

namespace UI.Views;
/// <summary>
/// Interaction logic for AccountView.xaml
/// </summary>
public partial class AccountView : UserControl
{
    private static Func<ChartPoint, string> _performanceLabelFormatter;

    public AccountView()
    {
        InitializeComponent();

        _performanceLabelFormatter = (point) =>
        {
            switch (Tag)
            {
                case PerfomanceTimeFrame.Daily:
                    return Math.Round(point.Y, 2).ToString() + " % " + new DateTime((long)point.X).ToString(" MM/dd/yyyy");
                case PerfomanceTimeFrame.Monthly:
                    return Math.Round(point.Y, 2).ToString() + " % " + new DateTime((long)point.X).ToString(" MMMM yyyy");
                case PerfomanceTimeFrame.Yearly:
                    return Math.Round(point.Y, 2).ToString() + " % " + new DateTime((long)point.X).ToString(" yyyy");
            }
            throw new Exception("it should return one of the previous, else...something went really wrong");
        };
    }

    private void OnAccountViewLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)DataContext;
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
        var dataContext = (AccountViewModel)DataContext;
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
        var dataContext = (AccountViewModel)DataContext;

        switch(timeFramePressedButtonName)
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
        var performanceLineSeries = (LineSeries)account_performance_line_chart.Series[0];
        performanceLineSeries.LabelPoint = _performanceLabelFormatter;
    }

    private void OnFilterPerformanceControlLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)DataContext;
        options_list_view.SetBinding(ListView.ItemsSourceProperty, "PerformanceViewModel.AccountPerformance");
    }

    private void OnSelectedDateFilterHandler(object sender, MouseButtonEventArgs e)
    {
        var senderRef = (TextBlock)sender;

        var dataContext = (AccountViewModel)DataContext;
        dataContext.PerformanceViewModel.FilterAccountPeroformanceByDateCommand.Execute((long)(double)senderRef.Tag);
    }
}
