using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace UI.Views;
/// <summary>
/// Interaction logic for AccountView.xaml
/// </summary>
public partial class AccountView : UserControl
{
    public AccountView()
    {
        InitializeComponent();
    }

    private void ShowFilterPerformanceByDateOptionsHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["expand_filter_performance_by_date_control_storyboard"];
        sBoard.Begin();
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

    private void OnAccountViewLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (AccountViewModel)DataContext;
        dataContext.LoadDailyPerformanceCommand.Execute(null);
    }
}
