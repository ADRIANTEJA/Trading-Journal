using API.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace UI.Windows;
/// <summary>
/// Interaction logic for DeleteStrategyWarningWindow.xaml
/// </summary>
public partial class DeleteStrategyWarningWindow : Window
{
    private readonly IEventAggregator _eventAggregator;

    private readonly int _contextStrategyId;

    public DeleteStrategyWarningWindow(int contextStrategyId)
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
        _contextStrategyId = contextStrategyId;
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void CloseProgramHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CancelButtonClickHandler(object sender, RoutedEventArgs e) => Close();

    private void OkButtonClickHandler(object sender, RoutedEventArgs e)
    {
        _eventAggregator.GetEvent<DeleteStrategyClickEvent>().Publish(_contextStrategyId);
        Close();
    }
}
