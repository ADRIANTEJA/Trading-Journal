using System.Windows;
using System.Windows.Input;

namespace UI.Windows;
/// <summary>
/// Interaction logic for TradeCostsWindow.xaml
/// </summary>
public partial class TradeCostsWindow : Window
{
    public TradeCostsWindow()
    {
        InitializeComponent();
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }
}
