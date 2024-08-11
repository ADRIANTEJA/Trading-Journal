using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UI.Windows;
/// <summary>
/// Interaction logic for ShowImageWindow.xaml
/// </summary>
public partial class ShowImageWindow : Window
{
    public ShowImageWindow(ImageSource imageSource)
    {
        InitializeComponent();

        show_image_control.Source = imageSource;
    }

    private void OnShowImageWindowHandler(object sender, RoutedEventArgs e)
    {
        var resizeIconFullscreenRef =
            (Image)resize_button.Template.FindName("resize_fullscreen_icon", resize_button);

        var resizeIconRef = (Image)resize_button.Template.FindName("resize_icon", resize_button);

        resizeIconFullscreenRef.Visibility = Visibility.Visible;
        resizeIconRef.Visibility = Visibility.Hidden;
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void CloseProgramHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void ResizeMainWindowHandler(object sender, RoutedEventArgs e)
    {
        var resizeIconFullscreenRef =
            (Image)resize_button.Template.FindName("resize_fullscreen_icon", resize_button);

        var resizeIconRef = (Image)resize_button.Template.FindName("resize_icon", resize_button);

        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            resizeIconFullscreenRef.Visibility = Visibility.Visible;
            resizeIconRef.Visibility = Visibility.Hidden;
        }
        else
        {
            WindowState = WindowState.Maximized;
            resizeIconFullscreenRef.Visibility = Visibility.Hidden;
            resizeIconRef.Visibility = Visibility.Visible;
        }
    }
}
