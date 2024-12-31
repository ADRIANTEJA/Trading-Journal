using System.Globalization;
using System.Windows.Data;
using UI.Common.Helpers;
using static MainModule.Common.Enums;

namespace UI.Common.Converters;

public class BinaryToStatusBrushConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        TradeStatus status = (TradeStatus)values[0];
        double openPrice = (double)values[1];
        double? closePrice = values[2] as double?;
        TradeSide side = (TradeSide)values[3];
        double volume = (double)values[4];
        double swap = (double)values[5];
        double spread = (double)values[6];
        double commission = (double)values[7];
        double otherCosts = (double)values[8];

        if (status == TradeStatus.Open) return ResourceAccessHelper.WarningYellowBrush;
        else
        {
            switch (side)
            {
                case TradeSide.Long:
                    if (volume * closePrice >= (volume * openPrice) + swap + spread + commission + otherCosts) 
                        return ResourceAccessHelper.GreenBrushRef;
                    else return ResourceAccessHelper.SalmonBrushRef;
                case TradeSide.Short:
                    if (volume * closePrice <= (volume * openPrice) + swap + spread + commission + otherCosts) 
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
