using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
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
        catch (FormatException) 
        {
            return false; 
        }
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

    public static bool IsWonTrade(Trade trade)
    {
        switch (trade.Side)
        {
            case TradeSide.Long:
                if (trade.Volume * trade.ClosePrice >= (trade.Volume * trade.OpenPrice)
                    + trade.Swap + trade.Spread + trade.Commission + trade.OtherCosts)
                    return true;

                else return false;
            case TradeSide.Short:
                if (trade.Volume * trade.ClosePrice <= (trade.Volume * trade.OpenPrice)
                    + trade.Swap + trade.Spread + trade.Commission + trade.OtherCosts)
                    return true;

                else return false;
        }

        return false;
    }

    public static bool ExportDataBaseFile(string fileName)
    {
        var OpenFolderDialog = new OpenFolderDialog();

        if (OpenFolderDialog.ShowDialog() == true)
        {
            string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string destinationFilePath = Path.Combine(OpenFolderDialog.FolderName, fileName);

            if (File.Exists(destinationFilePath))
            {
                var result = MessageBox.Show("The file already exists. Do you want to overwrite it?", "Confirm Overwrite", 
                                             MessageBoxButton.YesNo, 
                                             MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return false;
                }
            }

            try
            {
                File.Copy(sourceFilePath, destinationFilePath, true);
                MessageBox.Show("File exported successfully!", "Success", 
                           MessageBoxButton.OK, 
                           MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                var logger = App.AppHost!.Services.GetRequiredService<ILogger>();
                logger.LogError(ex, "{Message} {Timestamp} {Context}", ex.Message, DateTime.Now.ToString(), $"Thrown on line 119 class {nameof(MiscFunctions)}");

                MessageBox.Show($"An error occurred while exporting the file: {ex.Message}", 
                                "Error", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
                return false;
            }
        }
        return false;
    }

    public static bool ImportDataBaseFile(string fileName)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Database files (*.db)|*.db",
            Title = "Select a Database File"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string sourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string selectedFilePath = openFileDialog.FileName;

            try
            {
                File.Copy(selectedFilePath, sourceFilePath, true);
                MessageBox.Show("File imported successfully!", 
                                "Success", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);

                return true;
            }
            catch (Exception ex)
            {
                var logger = App.AppHost!.Services.GetRequiredService<ILogger>();
                logger.LogError(ex, "{Message} {Timestamp} {Context}", ex.Message, DateTime.Now.ToString(), $"Thrown on line 157 class {nameof(MiscFunctions)}");

                MessageBox.Show($"An error occurred while importing the file: {ex.Message}", 
                                 "Error", 
                                 MessageBoxButton.OK, 
                                 MessageBoxImage.Error);
                return false;
            }
        }
        return false;
    }

    public static double CalculatePercentage(double percentage, double total)
    {
        return (percentage / 100) * total;
    }
}
