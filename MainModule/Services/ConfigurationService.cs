using MainModule.Common;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MainModule.Services;

public class ConfigurationService : IConfigurationService
{
    public IConfiguration GetConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.AppSettingsFileName);
#if DEBUG
        builder.AddJsonFile(Constants.AppSettingsDevelopmentFileName, optional: true, reloadOnChange: true);

#else   
        builder.AddJsonFile(Constants.AppSettingsProductionFileName, optional:true, reloadOnChange: true);

#endif
        return builder.Build();
    }
}
