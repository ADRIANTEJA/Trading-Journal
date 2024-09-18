using System.Globalization;
using System.Windows.Data;
using UI.Common.Helpers;

namespace UI.Common.Converters;

public class BinaryToStatusBrushConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        int IsOpen = (int)values[0];
        double OpenPrice = (double)values[1];
        double ClosePrice = (double)values[2];
        int IsLong = (int)values[3];

        if (IsOpen == 1) return ResourceAccessHelper.WarningYellowBrush;
        else
        {
            switch(IsLong)
            {
                case 1:
                    if (ClosePrice >= OpenPrice) return ResourceAccessHelper.GreenBrushRef;
                    else return ResourceAccessHelper.SalmonBrushRef;
                case 0:
                    if (ClosePrice <= OpenPrice) return ResourceAccessHelper.GreenBrushRef;
                    else return ResourceAccessHelper.SalmonBrushRef;
            }
        }

        throw new ArgumentException("Unsupported binding arguments");
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Cannot Convert Back");
    }
}
