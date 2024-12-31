using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static MainModule.Common.Enums;

namespace UI.Common.Converters;

public class AccountInBankruptVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isBankrupt = (AccountBankruptcyStatus)value;

        return isBankrupt == AccountBankruptcyStatus.Bankrupt ? Visibility.Visible : Visibility.Hidden;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
