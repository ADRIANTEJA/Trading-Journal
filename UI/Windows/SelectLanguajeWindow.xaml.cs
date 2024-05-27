using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Services;

namespace UI.Windows;
/// <summary>
/// Interaction logic for SelectLanguajeWindow.xaml
/// </summary>
public partial class SelectLanguajeWindow : Window
{
    private IUIConfigurationService _uiConfigService;

    public SelectLanguajeWindow(IUIConfigurationService uiConfigService)
    {
        InitializeComponent();
        _uiConfigService = uiConfigService;
    }

    private void ReflectSelectedLanguageHandler(object sender, RoutedEventArgs e)
    {
        var config = _uiConfigService.GetConfiguration();

        var optionControlRef = (Grid)FindName(config["Language"]);
        var radio_button_ref = (RadioButton)optionControlRef.FindName("radio_button");
        radio_button_ref.IsChecked = true;
    }

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }
}
