using System.Windows;
using System.Windows.Input;

namespace UI.Windows;
/// <summary>
/// Interaction logic for NoAccountCreatedPromptWindow.xaml
/// </summary>
public partial class NoAccountCreatedPromptWindow : Window
{
    public NoAccountCreatedPromptWindow()
    {
        InitializeComponent();
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void CloseProgramHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OkButtonClickHandler(object sender, RoutedEventArgs e) => Close();
}
