using MainModule.DataModel;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using UI.Common.Utils;

namespace UI.Common.Converters;

public class DailyLossConstraintDatesConverter : IValueConverter
{
    internal class TradeDataBundle
    {
        public long CloseDateTicks { get; set; }

        public double ROI { get; set; }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var contextStrategy = (Strategy)value;

        if (contextStrategy != null)
        {
            var trades = (from trade in App.AppHost!.Services.GetRequiredService<HomeViewModel>().Trades
                          where trade.StrategyName != null
                          && trade.IsOpen == 0
                          && trade.StrategyName == contextStrategy.Name
                          && !MiscFunctions.IsWonTrade(trade)
                          select new TradeDataBundle
                          {
                              CloseDateTicks = (long)trade.CloseDate!,
                              ROI = (double)trade.Roi!
                          }).ToList();

            var stringDates = trades
                .Select(x => new { Date = new DateTime(x.CloseDateTicks), x.ROI })
                .GroupBy(x => new { x.Date.Year, x.Date.Month, x.Date.Day })
                .Where(x => x.Sum(x => x.ROI) < contextStrategy.MaxDailyLoss * -1)
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


