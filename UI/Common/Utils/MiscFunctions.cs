using LiveCharts;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static MainModule.Common.Enums;

namespace UI.Common.Utils;

public static class MiscFunctions
{
    public static void RestartApplication()
    {
        var currentExecutablePath = Environment.ProcessPath;
        Process.Start(currentExecutablePath!);
        Application.Current.Shutdown();
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

    public static BitmapSource ByteArrayToBitmapSource(byte[] buffer)
    {
        BitmapSource bitmap = null;

        if (buffer != null)
        {
            using var stream = new MemoryStream(buffer);
            bitmap = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        }

        return bitmap;
    }
    /// <summary>
    /// opens a dialog interface and returns the selected image file
    /// path, else returns a empty string
    /// </summary>
    /// <returns></returns>
    public static string GetImagePathFromDisk()
    {
        OpenFileDialog dlg = new()
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *.bmp"
        };

        if (dlg.ShowDialog() == true) return dlg.FileName;
        else return "";
    }
}
