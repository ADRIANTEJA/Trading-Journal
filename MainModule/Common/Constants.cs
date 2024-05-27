using System.IO;

namespace MainModule.Common;

public static class Constants
{
	private static readonly double smallFontSize = 12;
    /// <summary>
    /// a small font size value of 12px
    /// </summary>
    public static double SmallFontSize => smallFontSize;

	private static readonly double mediumFontSize = 14;
    /// <summary>
    /// a medium font size value of 14px
    /// </summary>
    public static double MediumFontSize => mediumFontSize;

	private static readonly double largeFontSize = 16;
	/// <summary>
	/// a large font size value of 16px
	/// </summary>
	public static double LargeFontSize => largeFontSize;
	
	private static readonly string uiUserSettingsFileName = "UISettingsUser.json";
	/// <summary>
	/// the name of the configuration json file that contains user UI preferences
	/// </summary>
	public static string UIUserSettingsFileName => uiUserSettingsFileName;
	/// <summary>
	/// the path to the configuration json file that contains user UI preferences
	/// </summary>
	public static string UIUserSettingsFilePath
	{
		get 
		{ 
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                            applicationDataFolderName,
                                                            uiUserSettingsFileName);
        }
	}

	private static readonly string appSettingsFileName = "AppSettings.json";
	/// <summary>
	/// the name of the main configuartion json file that contains application settings
	/// </summary>
	public static string AppSettingsFileName => appSettingsFileName;

	private static readonly string appSettingsDevelopmentFileName = "AppSettings.Development.json";
	/// <summary>
	/// the name of the configuration json file that contains application settings
	/// used only at development time
	/// </summary>
	public static string AppSettingsDevelopmentFileName => appSettingsDevelopmentFileName;

	private static readonly string appSettingsProductionFileName = "AppSettings.Production.json";
	/// <summary>
	/// the name of the configuration json file that contains application settings
	/// to be used once the application is on production
	/// </summary>
	public static string AppSettingsProductionFileName => appSettingsProductionFileName;

	private static readonly string applicationDataFolderName = "Asset Ops";
	/// <summary>
	/// the name of the folder created to storage user settings files
	/// </summary>
	public static string ApplicationDataFolderName => applicationDataFolderName;

	private static readonly Uri darkThemeDictionarySource = new("Common/Resources/Dictionaries/DarkThemeDictionary.xaml",
																UriKind.Relative);
	/// <summary>
	/// source of the xaml dictionary where dark theme specific resources are storaged
	/// </summary>
	public static Uri DarkThemeDictionarySource => darkThemeDictionarySource;

	private static readonly Uri lightThemeDictionarySource = new("Common/Resources/Dictionaries/LightThemeDictionary.xaml",
															     UriKind.Relative);
	/// <summary>
	/// source of the xaml dictionary where the light theme specific resources are storaged
	/// </summary>
	public static Uri LightThemeDictionarySource => lightThemeDictionarySource;

	private static readonly string languageDictionariesFolderPath = "Common/Resources/Dictionaries/Language/";

	public static string LanguageDictionariesFolderPath => languageDictionariesFolderPath;
}
