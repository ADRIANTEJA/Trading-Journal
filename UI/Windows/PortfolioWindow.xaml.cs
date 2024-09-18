using MainModule.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace UI.Windows;
/// <summary>
/// Interaction logic for PortfolioWindow.xaml
/// </summary>
public partial class PortfolioWindow : Window
{
    public PortfolioWindow()
    {
        InitializeComponent();
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OnPortfolioWindowLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (SymbolViewModel)DataContext;
        dataContext.LoadSymbolsCommand.Execute(null);
    }
}
