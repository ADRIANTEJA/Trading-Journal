using Microsoft.Extensions.Configuration;
using System.IO;

namespace MainModule.Services;

public class DataAccessConfiguration : IDataAccessConfiguration
{
    public IConfiguration GetConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json");
#if DEBUG
        builder.AddJsonFile("AppSettings.Development.json", optional: true, reloadOnChange: true);

#else   
        builder.AddJsonFile("AppSettings.Production.json", optional:true, reloadOnChange: true);

#endif

        return builder.Build();
    }
}
