using MainModule.DataModel;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using UI.Common.Utils;
using static MainModule.Common.Enums;

namespace UI.Common.Converters;

public class TradeRiskConstraintWarningVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var contextTrade = (Trade)value;
        var strategyViewModel = App.AppHost!.Services.GetRequiredService<StrategyViewModel>();

        if (contextTrade == null) return Visibility.Hidden;

        var tempList = (from strategy in strategyViewModel.Strategies
                        where strategy.Name == contextTrade.StrategyName
                        select strategy).ToList();


        if (contextTrade.Status == TradeStatus.Closed
            && tempList.Count > 0
            && !MiscFunctions.IsWonTrade(contextTrade) 
            && contextTrade.Roi < MiscFunctions.CalculatePercentage(tempList[0].MaxTradeRisk,
                                                                    contextTrade.AccountBalance!.Value) * -1) return Visibility.Visible;

        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
