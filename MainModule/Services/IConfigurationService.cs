using Microsoft.Extensions.Configuration;

namespace MainModule.Services;

public interface IConfigurationService
{
    IConfiguration GetConfiguration();
}
