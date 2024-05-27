using MainModule.Common;
using MainModule.Common.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;
using UI.Settings;

namespace UI.Services;

public class UIConfigurationService : IUIConfigurationService
{
    private readonly string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                                                    Constants.ApplicationDataFolderName);

    public void ApplySettings(UISettings uiSettingsModel)
    {
        if (!File.Exists(Constants.UIUserSettingsFilePath)) 
            JsonFileUtils.SerializeJsonFile(uiSettingsModel, Constants.UIUserSettingsFilePath);
    }

    public IConfiguration GetConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(Constants.UIUserSettingsFileName, optional: true, reloadOnChange: true);

        return builder.Build();
    }
}
