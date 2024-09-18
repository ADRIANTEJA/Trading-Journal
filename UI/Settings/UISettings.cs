
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows.Markup;
using System.Windows;
using UI.Services;

namespace UI.Settings;
/// <summary>
/// UI settings model class with default yet configurable
/// values for user preferences
/// </summary>
public class UISettings
{
    private bool isDarkThemeOn = true;
    private LanguageOption launguage = LanguageOption.English;
    private FontSizeOption fontSize = FontSizeOption.Medium;

    /// <summary>
    /// defines wether the dark theme is on or off
    /// </summary>
    public bool IsDarkThemeOn 
    { 
        get { return isDarkThemeOn; }
        set { isDarkThemeOn = value; }
    }
    /// <summary>
    /// defines in which languaje is the app showing 
    /// </summary> 
    public LanguageOption Language 
    { 
        get { return launguage; } 
        set { launguage = value; }
    }
    /// <summary>
    /// defines the current font size of the app
    /// small, normal or large
    /// </summary> 
    public FontSizeOption FontSize 
    { 
        get { return fontSize; }
        set { fontSize = value; }
    }
    
    public enum LanguageOption 
    { 
        English, 
        Spanish,
        Russian,
        French,
        Italian,
        Japanese,
        Chinese,
        German,
        Portuguese
    }

    public enum FontSizeOption { small, Medium, Large }

    public static void ChangeElementCulture(string cultureCode, FrameworkElement element)
    {
        var culture = new CultureInfo(cultureCode);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "dd/MM/yyyy hh.mm tt";

        element.Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
    }

    public static void ApplyElementCultureSettings(FrameworkElement element)
    {
        var config = App.AppHost!.Services.GetRequiredService<IUIConfigurationService>().GetConfiguration();

        switch (config["Language"])
        {
            case nameof(Language.English):
                ChangeElementCulture("en-US", element);
                break;
            case nameof(Language.Spanish):
                ChangeElementCulture("es-ES", element);
                break;
            case nameof(Language.Japanese):
                ChangeElementCulture("ja-JP", element);
                break;
            case nameof(Language.Russian):
                ChangeElementCulture("ru-RU", element);
                break;
            case nameof(Language.Chinese):
                ChangeElementCulture("zh-CN", element);
                break;
            case nameof(Language.French):
                ChangeElementCulture("fr-FR", element);
                break;
            case nameof(Language.German):
                ChangeElementCulture("de-DE", element);
                break;
            case nameof(Language.Portuguese):
                ChangeElementCulture("pt-PT", element);
                break;
            case nameof(Language.Italian):
                ChangeElementCulture("it-IT", element);
                break;
        }
    }
}
