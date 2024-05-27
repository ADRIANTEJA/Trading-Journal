using MainModule.Common.Utils;
using UI.Settings;

namespace MainModule.Tests;

public class JsonFileUtilsTest
{
    [Fact]
    public void SerializeJsonFile_ShouldSerialize()
    {
        UISettings uiSettings = new()
        {
            IsDarkThemeOn = true,
            FontSize = UISettings.FontSizeOption.Medium,
            Language = UISettings.LanguageOption.English
        };

        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                             "Asset Ops", "UISettingsDefault.json");

        JsonFileUtils.SerializeJsonFile(uiSettings, path);

        Assert.True(File.Exists(path));
    }

    [Fact]
    public void SerializeJsonFile_ShouldDeserialize()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                                         "Asset Ops", "UISettingsDefault.json");

        var output = JsonFileUtils.DeserializeJsonFile<UISettings>(path);

        Assert.True(output != null);
    }
}
