using MainModule.Common;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace UI.Common.Utils;

public static class MiscFunctions
{
    public static void RestartApplication()
    {
        var currentExecutablePath = Environment.ProcessPath;
        Process.Start(currentExecutablePath!);
        Application.Current.Shutdown();
    }

    public static void HandleUISettingsFileError()
    {
        MessageBox.Show((string)Application.Current.FindResource("ui_settings_file_error_message"),
                            (string)Application.Current.FindResource("ui_settings_file_error_header"),
                            MessageBoxButton.OK, MessageBoxImage.Error);

        File.Delete(Constants.UIUserSettingsFilePath);
        RestartApplication();
    }

    public static bool CheckInputIsNumeric(string content)
    {
        try
        {
            if (content.StartsWith('-') || content.Contains('e')) throw new FormatException();
            double.Parse(content);
            return true;
        }
        catch (FormatException) { return false; }
    }
}
