using MainModule.DataModel;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows.Data;
using UI.Common.Utils;

namespace UI.Common.Converters;

public class DailyLossConstraintDatesConverter : IValueConverter
{
    internal class TradeDataBundle
    {
        public long CloseDateTicks { get; set; }

        public double ROI { get; set; }

        public double accountBalance { get; set; }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var contextStrategy = (Strategy)value;
        var vmTrades = App.AppHost!.Services.GetRequiredService<HomeViewModel>().Trades;

        if (contextStrategy != null)
        {
            var trades = (from trade in vmTrades
                          where !string.IsNullOrEmpty(trade.StrategyName)
                          && trade.IsOpen == 0
                          && trade.StrategyName == contextStrategy.Name
                          && !MiscFunctions.IsWonTrade(trade)
                          select new TradeDataBundle
                          {
                              CloseDateTicks = trade.CloseDate!.Value,
                              ROI = trade.Roi!.Value,
                              accountBalance = trade.AccountBalance
                          }).ToList();

            if (trades.Count == 0) return new List<string>();

            var dayFirstTrade = trades
                .OrderBy(trade => trade.CloseDateTicks)
                .FirstOrDefault();

            var stringDates = trades
                .Select(x => new { Date = new DateTime(x.CloseDateTicks), x.ROI })
                .GroupBy(x => new { x.Date.Year, x.Date.Month, x.Date.Day })
                .Where(x => x.Sum(x => x.ROI) < MiscFunctions.CalculatePercentage(contextStrategy.MaxDailyLoss,
                                                                                  dayFirstTrade!.accountBalance) * -1)
                .Select(g => new DateTime(g.Key.Year, g.Key.Month, g.Key.Day).ToString("dd/MM/yyyy")).ToList();

            return stringDates;
        }

        return new List<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}


