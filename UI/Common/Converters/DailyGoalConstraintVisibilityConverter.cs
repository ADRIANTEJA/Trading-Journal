using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows.Data;
using UI.Common.Utils;
using MainModule.DataModel;
using System.Windows;

namespace UI.Common.Converters;

public class DailyGoalConstraintVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var contextTrade = (Trade)value;

        if (contextTrade == null || contextTrade.IsOpen == 1) return Visibility.Hidden;

        string tradeDate = new DateTime(contextTrade.CloseDate!.Value).ToString("dd/MM/yyyy");

        var tradeStrategy = (from strategy in App.AppHost!.Services.GetRequiredService<StrategyViewModel>().Strategies
                             where strategy.Name == contextTrade.StrategyName
                             select strategy).ToList();
        
        if (tradeStrategy.Count == 0) return Visibility.Hidden;

        var trades = (from trade in App.AppHost!.Services.GetRequiredService<HomeViewModel>().Trades
                      where !string.IsNullOrEmpty(trade.StrategyName)
                      && trade.IsOpen == 0
                      && trade.StrategyName == tradeStrategy[0].Name
                      && MiscFunctions.IsWonTrade(trade)
                      && tradeDate == new DateTime(trade.CloseDate!.Value).ToString("dd/MM/yyyy")
                      select trade).ToList();

        if (trades.Count == 0) return Visibility.Hidden;

        var dayFirstTrade = trades
            .OrderBy(trade => trade.CloseDate!.Value)
            .FirstOrDefault();

        double totalROI = trades.Sum(x => x.Roi!.Value);

        if (totalROI >= MiscFunctions.CalculatePercentage(tradeStrategy[0].DailyGoal,
                                                          dayFirstTrade!.AccountBalance)) return Visibility.Visible;
        else return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
