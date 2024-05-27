using Microsoft.Extensions.Configuration;
using UI.Settings;

namespace UI.Services;

public interface IUIConfigurationService
{
    void ApplySettings(UISettings uiSettingsModel);
    IConfiguration GetConfiguration();   
}
