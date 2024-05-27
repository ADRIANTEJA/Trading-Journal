using MainModule.Common;
using MainModule.Common.Utils;
using System.Windows;
using System.Windows.Controls;
using UI.Common.Utils;
using UI.Settings;

namespace UI.Controls.Sliders.MainWindow;
/// <summary>
/// Interaction logic for FontSizeSlider.xaml
/// </summary>
public partial class FontSizeSlider : Grid
{
    public FontSizeSlider()
    {
        InitializeComponent();
    }

    private void FontSizeChangeHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var font_size_slider_ref = (Slider)sender;

        switch (font_size_slider_ref.Value)
        {
            case 0:
                Application.Current.Resources["font_size"] = Constants.SmallFontSize;
                SaveSettings(UISettings.FontSizeOption.small);
                break;
            case 1:
                Application.Current.Resources["font_size"] = Constants.MediumFontSize;
                SaveSettings(UISettings.FontSizeOption.Medium);
                break;
            case 2:
                Application.Current.Resources["font_size"] = Constants.LargeFontSize;
                SaveSettings(UISettings.FontSizeOption.Large);
                break;
        }

    }

    private void SaveSettings(UISettings.FontSizeOption fontSize)
    {
        try
        {
            var config = (UISettings)JsonFileUtils.DeserializeJsonFile<UISettings>(Constants.UIUserSettingsFilePath);
            config.FontSize = fontSize;
            JsonFileUtils.SerializeJsonFile(config, Constants.UIUserSettingsFilePath);
        }
        catch { MiscFunctions.HandleUISettingsFileError(); }
    }
}
