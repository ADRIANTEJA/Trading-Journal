using System.Windows;
using System.Windows.Input;

namespace UI.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) { DragMove(); }
    }

    private void CloseProgramHandler(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MinimizeMainWindowHandler(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void ResizeMainWindowHandler(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized) 
        {
           WindowState = WindowState.Normal;
        }
        else WindowState = WindowState.Maximized;
    }
}