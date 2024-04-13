
using Microsoft.Extensions.Configuration;

namespace MainModule.Services;

public interface IDataAccessConfiguration
{
    IConfiguration GetConfiguration();
}
