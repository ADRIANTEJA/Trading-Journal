
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
}
