using System.Windows;
using UI.Services;
using UI.Settings;

namespace MainModule.Tests;

public class UIConfigurationServiceTests
{
    UIConfigurationService uiConfigService = new();

    [Fact]
    public void GetConfiguration_ShouldGetConfiguration()
    {
        try
        {
            Assert.True(uiConfigService.GetConfiguration()["IsDarkThemeOn"] != null
                        && uiConfigService.GetConfiguration()["Language"] != null
                        && uiConfigService.GetConfiguration()["FontSize"] != null);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Assert.Fail(); 
        }
    }
}
