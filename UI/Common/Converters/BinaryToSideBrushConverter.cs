using System.Globalization;
using System.Windows.Data;
using UI.Common.Helpers;

namespace UI.Common.Converters;

public class BinaryToSideBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int castedValue = (int)value;

        if (castedValue == 1) return ResourceAccessHelper.GreenBrushRef;
        else return ResourceAccessHelper.SalmonBrushRef;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
