using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Common.Converters;

public class AccountInBankruptVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isBankrupt = (int)value;

        return isBankrupt == 1 ? Visibility.Visible : Visibility.Hidden;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
