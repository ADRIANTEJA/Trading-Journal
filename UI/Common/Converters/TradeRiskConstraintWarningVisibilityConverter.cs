using MainModule.DataModel;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Common.Converters;

public class TradeRiskConstraintWarningVisibilityConverter : IValueConverter
{
    // dont delete just uncomment after done with making the UI
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //var contextTrade = (Trade)value;
        //var strategyViewModel = App.AppHost!.Services.GetRequiredService<StrategyViewModel>();

        //var tempList = (from strategy in strategyViewModel.Strategies
        //                where strategy.Name == contextTrade.StrategyName
        //                select strategy).ToList();


        //if (contextTrade.IsOpen == 0
        //    && tempList.Count > 0
        //    && contextTrade.Roi < 0 
        //    && contextTrade.Roi < tempList[0].MaxTradeRisk * -1) return Visibility.Visible;

        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
