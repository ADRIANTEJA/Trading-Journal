using MainModule.Common;
using MainModule.Common.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Settings;

namespace UI.Controls.Buttons.MainWindow;
/// <summary>
/// Interaction logic for ThemeOptions.xaml
/// </summary>
public partial class ThemeOptions : Grid
{
    public ThemeOptions()
    {
        InitializeComponent();
    }

    private void SwitchToLightThemeHandler(object sender, RoutedEventArgs e)
    {
        Application.Current.Resources.MergedDictionaries.Add(new() { Source = Constants.LightThemeDictionarySource });
        Application.Current.Resources.MergedDictionaries.Remove(new() { Source = Constants.DarkThemeDictionarySource });

        light_theme_button.BorderBrush = (SolidColorBrush)Application.Current.FindResource("theme_background_inverse_brush");
        dark_theme_button.BorderBrush = null;

        SaveThemeSettings(false);
    }

    private void SwitchToDarkThemeHandler(object sender, RoutedEventArgs e)
    {
        Application.Current.Resources.MergedDictionaries.Add(new() { Source = Constants.DarkThemeDictionarySource });
        Application.Current.Resources.MergedDictionaries.Remove(new() { Source = Constants.LightThemeDictionarySource });

        dark_theme_button.BorderBrush = (SolidColorBrush)Application.Current.FindResource("theme_background_inverse_brush");
        light_theme_button.BorderBrush = null;

        SaveThemeSettings(true);
    }

    private void SaveThemeSettings(bool isDarkThemeOn)
    {
        try
        {
            var settings = (UISettings?)JsonFileUtils.DeserializeJsonFile<UISettings>(Constants.UIUserSettingsFilePath);
            settings.IsDarkThemeOn = isDarkThemeOn;

            JsonFileUtils.SerializeJsonFile(settings, Constants.UIUserSettingsFilePath);
        }
        catch { MiscFunctions.HandleUISettingsFileError(); }
    }
}
