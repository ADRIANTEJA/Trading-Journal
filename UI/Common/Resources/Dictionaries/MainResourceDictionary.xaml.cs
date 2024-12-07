using System.Windows;

namespace UI.Common.Resources.Dictionaries;

public partial class MainResourceDictionary : ResourceDictionary
{
    private static readonly Func<double, string> roiValueLabelFormatter = (value) =>
    {
        if (value != 0)
        {
            string stringValue = Math.Round(value, 0).ToString();
            return string.Concat(stringValue.AsSpan(0, stringValue.Length - 3), " K");
        }

        return 0.ToString();
    };

    public MainResourceDictionary()
    {
        Add("roi_value_label_formatter", roiValueLabelFormatter);
    }
}
