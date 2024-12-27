using API.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using UI.Events;

namespace UI.Windows;
/// <summary>
/// Interaction logic for DeleteAccountWarningWindow.xaml
/// </summary>
public partial class DeleteAccountWarningWindow : Window
{
    private readonly IEventAggregator _eventAggregator;

    private readonly int _contextAccountId;

    public DeleteAccountWarningWindow(int contextAccountId)
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
        _contextAccountId = contextAccountId;
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
        _eventAggregator.GetEvent<AccountDeletedEvent>().Publish(_contextAccountId);
        Close();
    } 
}
