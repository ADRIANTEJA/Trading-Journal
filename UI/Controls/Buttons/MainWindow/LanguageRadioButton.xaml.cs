using MainModule.Common;
using MainModule.Common.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Common.Utils;
using UI.Services;
using UI.Settings;

namespace UI.Controls.Buttons.MainWindow;
/// <summary>
/// Interaction logic for LanguageRadioButton.xaml
/// </summary>
public partial class LanguageRadioButton : Grid
{
    public LanguageRadioButton()
    {
        InitializeComponent();
    }

    private void SwitchLanguageHandler(object sender, RoutedEventArgs e)
    {
        var config = App.AppHost!.Services.GetRequiredService<IUIConfigurationService>().GetConfiguration();
        
        switch (Tag)
        {
            case "English":
                ApplyAndSaveSettings(config, "EnglishDictionary.xaml", UISettings.LanguageOption.English);
                break;
            case "Español":
                ApplyAndSaveSettings(config, "SpanishDictionary.xaml", UISettings.LanguageOption.Spanish);
                break;
            case "中国人":
                ApplyAndSaveSettings(config, "ChineseDictionary.xaml", UISettings.LanguageOption.Chinese);
                break;
            case "日本語":
                ApplyAndSaveSettings(config, "JapaneseDictionary.xaml", UISettings.LanguageOption.Japanese);
                break;
            case "Русский":
                ApplyAndSaveSettings(config, "RussianDictionary.xaml", UISettings.LanguageOption.Russian);
                break;
            case "Deutsch":
                ApplyAndSaveSettings(config, "GermanDictionary.xaml", UISettings.LanguageOption.German);
                break;
            case "Português":
                ApplyAndSaveSettings(config, "PortugueseDictionary.xaml", UISettings.LanguageOption.Portuguese);
                break;
            case "Français":
                ApplyAndSaveSettings(config, "FrenchDictionary.xaml", UISettings.LanguageOption.French);
                break;
            case "Italiano":
                ApplyAndSaveSettings(config, "ItalianDictionary.xaml", UISettings.LanguageOption.Italian);
                break;
        }
    }
    /// <summary>
    /// Apply and saves language configurations based on settings written in the user UI settings
    /// configuration json, the dictionaryFileName parameter is case sensitive
    /// </summary>
    /// <param name="config"></param>
    /// <param name="dictionaryFileName"></param>
    private void ApplyAndSaveSettings(IConfiguration config, string dictionaryFileName, UISettings.LanguageOption language)
    {
        Application.Current.Resources.MergedDictionaries.Add(new()
        { Source = new(Constants.LanguageDictionariesFolderPath + dictionaryFileName, UriKind.Relative) });
        Application.Current.Resources.MergedDictionaries.Remove(new()
        { Source = new(Constants.LanguageDictionariesFolderPath + config["Language"] + "Dictionary.xaml", UriKind.Relative) });

        try
        {
            var uiSettings = (UISettings)JsonFileUtils.DeserializeJsonFile<UISettings>(Constants.UIUserSettingsFilePath);
            uiSettings.Language = language;
            JsonFileUtils.SerializeJsonFile(uiSettings, Constants.UIUserSettingsFilePath);
        }
        catch { ErrorHandlers.HandleUISettingsFileError(); }
    }
}
