using System.Windows;
using System.Windows.Input;
using UI.Common.Helpers;
using UI.Views.ROICalculator;

namespace UI.Windows;
/// <summary>
/// Interaction logic for CalculatorWindow.xaml
/// </summary>
public partial class CalculatorWindow : Window
{
    public CalculatorWindow()
    {
        InitializeComponent(); 
    }

    private void OnWindowLoadedHandler(object sender, RoutedEventArgs e)
    {
        roi_button.Background = ResourceAccessHelper.GreenBrushRef;
        navigation_panel.Content = new ROICalculationView();
    }

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void NavigateToROIViewHandler(object sender, RoutedEventArgs e)
    {
        if(navigation_panel.Content.GetType() != typeof(ROICalculationView))
        {
            navigation_panel.Content = new ROICalculationView();
            roi_button.Background = ResourceAccessHelper.GreenBrushRef;
            target_price_button.Background = null;
        }
    }

    private void NavigateToTargetPriceViewHandler(object sender, RoutedEventArgs e)
    {
        if (navigation_panel.Content.GetType() != typeof(TargetPriceCaculationView))
        {
            navigation_panel.Content = new TargetPriceCaculationView();
            target_price_button.Background = ResourceAccessHelper.GreenBrushRef;
            roi_button.Background = null;
        }   
    }  
}
