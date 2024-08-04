using MainModule.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Services;

namespace UI.Windows;

public partial class MainWindow : Window
{
    private readonly IUIConfigurationService _uiConfigService;

    public MainWindow(IUIConfigurationService uiConfigService)
    { 
        InitializeComponent();
        _uiConfigService = uiConfigService;
    }

    private void OnWindowLoadedHandler(object sender, RoutedEventArgs e)
    {
        // this calls the method for loading UI settings, if the settings file is missing or bad written
        // it is deleted and recreated thus reseting the application's UI settings and restarting the app
        try { ApplyUIPreferencesOnStartup(); }
        catch { ErrorHandlers.HandleUISettingsFileError(); }

        home_button.Background = ResourceAccessHelper.GreenBrushRef;
    }

    public void ApplyUIPreferencesOnStartup()
    {
        _uiConfigService.ApplySettings(new());

        var config = _uiConfigService.GetConfiguration();

        var dark_theme_button_ref = (Border)theme_options.FindName("dark_theme_button");
        var light_theme_button_ref = (Border)theme_options.FindName("light_theme_button");

        switch (config["IsDarkThemeOn"])
        {
            case "True":
                Application.Current.Resources.MergedDictionaries.Add(new() { Source = Constants.DarkThemeDictionarySource });
                Application.Current.Resources.MergedDictionaries.Remove(new() { Source = Constants.LightThemeDictionarySource });

                dark_theme_button_ref.SetResourceReference(BorderBrushProperty, ResourceAccessHelper.ThemeBackgroundInverseBrushKey);
                break;
            case "False":
                Application.Current.Resources.MergedDictionaries.Add(new() { Source = Constants.LightThemeDictionarySource });
                Application.Current.Resources.MergedDictionaries.Remove(new() { Source = Constants.DarkThemeDictionarySource });

                light_theme_button_ref.SetResourceReference(BorderBrushProperty, ResourceAccessHelper.ThemeBackgroundInverseBrushKey);
                break;             
        }

        var font_size_slider_ref = (Slider)font_size_options.FindName("font_size_slider");

        switch (config["FontSize"])
        {
            case "Small":
                Application.Current.Resources["font_size"] = Constants.SmallFontSize;

                font_size_slider_ref.Value = 0;
                break;
            case "Medium":
                Application.Current.Resources["font_size"] = Constants.MediumFontSize;

                font_size_slider_ref.Value = 1;
                break;
            case "Large":
                Application.Current.Resources["font_size"] = Constants.LargeFontSize;

                font_size_slider_ref.Value = 2;
                break;
        }

        Application.Current.Resources.MergedDictionaries.Add(new()
        { Source = new(Constants.LanguageDictionariesFolderPath + config["Language"] + "Dictionary.xaml", UriKind.Relative) });
        Application.Current.Resources.MergedDictionaries.Remove(new() 
        { Source = new(Constants.LanguageDictionariesFolderPath + "EnglishDictionary.xaml", UriKind.Relative) });   
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove(); 
    }

    private void CloseProgramHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void ResizeMainWindowHandler(object sender, RoutedEventArgs e)
    {
        var resize_icon_fullscreen_ref = (Image)resize_button.Template.FindName("resize_fullscreen_icon", resize_button);
        var resize_icon_ref = (Image)resize_button.Template.FindName("resize_icon", resize_button);

        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            resize_icon_fullscreen_ref.Visibility = Visibility.Visible;
            resize_icon_ref.Visibility = Visibility.Hidden;
        }
        else
        {
            WindowState = WindowState.Maximized;
            resize_icon_fullscreen_ref.Visibility = Visibility.Hidden;
            resize_icon_ref.Visibility = Visibility.Visible;
        }
    }

    private void ShowSettingsSliderHandler(object sender, RoutedEventArgs e)
    {
        var sBoard = (Storyboard)Resources["show_slide_settings_menu_storyboard"];
        sBoard.Begin();
    }

    private void HideSettingsSliderHandler(object sender, RoutedEventArgs e)
    {
        var sBoard = (Storyboard)Resources["hide_slide_settings_menu_storyboard"];
        sBoard.Begin();

        var sBoardReverse = (Storyboard)Resources["hide_theme_options_storyboard"];
        sBoardReverse.Begin();
    }

    private void ShowThemeOptionsHandler(object sender, RoutedEventArgs e)
    {
        if (theme_options.Margin.Top == 5)
        {
            var sBoard = (Storyboard)Resources["show_theme_options_storyboard"];
            sBoard.Begin();          
        }
        else
        {
            var sBoardReverse = (Storyboard)Resources["hide_theme_options_storyboard"];
            sBoardReverse.Begin();
        }    
    }

    private void HoldNavButtonHighlightHandler(object sender, RoutedEventArgs e)
    {
        var senderButtonRef = (Button)sender;

        home_button.Background = null;
        account_button.Background = null;
        strategy_button.Background = null;

        senderButtonRef.Background = ResourceAccessHelper.GreenBrushRef;
    }

    private void OpenCalculatorWindowHandler(object sender, RoutedEventArgs e)
    {
        var calculatorWindow = new CalculatorWindow();
        calculatorWindow.ShowDialog();
    }

    private void OnNaviagtionPanelLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (MainNavigationHelper)DataContext;
        dataContext.NavigateToHomeCommand.Execute(null);
    }
}