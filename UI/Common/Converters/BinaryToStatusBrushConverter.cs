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
        double? ClosePrice = values[2] as double?;
        int IsLong = (int)values[3];
        double volume = (double)values[4];
        double swap = (double)values[5];
        double spread = (double)values[6];
        double commission = (double)values[7];
        double otherCosts = (double)values[8];

        if (IsOpen == 1) return ResourceAccessHelper.WarningYellowBrush;
        else
        {
            switch (IsLong)
            {
                case 1:
                    if (volume * ClosePrice >= (volume * OpenPrice) + swap + spread + commission + otherCosts) 
                        return ResourceAccessHelper.GreenBrushRef;
                    else return ResourceAccessHelper.SalmonBrushRef;
                case 0:
                    if (volume * ClosePrice <= (volume * OpenPrice) + swap + spread + commission + otherCosts) 
                        return ResourceAccessHelper.GreenBrushRef;
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
