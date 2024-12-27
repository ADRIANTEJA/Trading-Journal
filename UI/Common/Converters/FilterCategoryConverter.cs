using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows.Data;

namespace UI.Common.Converters;

public class FilterCategoryConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var symbolList = App.AppHost!.Services.GetRequiredService<SymbolViewModel>().Symbols;

        return symbolList.Where(symbol => symbol.AssetType == parameter.ToString()).ToList();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("cannot convert back");
    }
}
