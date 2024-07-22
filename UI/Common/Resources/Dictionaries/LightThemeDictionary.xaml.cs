using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Windows;

namespace UI.Common.Resources.Dictionaries;

public partial class LightThemeDictionary : ResourceDictionary
{
    public LightThemeDictionary()
    {
        Add("theme_line_chart_xaxes_style", new Axis[]
        {   
            new Axis()
            {
                Name = "Days",
                NameTextSize = 16,
                NamePaint = new SolidColorPaint(new(20, 20, 20)),
                Labeler = (double value) => new DateTime((long)value).ToString("dd/mm/yyyy"),
                LabelsPaint = new SolidColorPaint(new(20, 20, 20)),
            }
        });

        Add("theme_line_chart_yaxes_style", new Axis[]
        {
            new()
            {
                Name = "ROI",
                NameTextSize = 16,
                SeparatorsPaint = null,
                MinStep = 1,
                NamePaint = new SolidColorPaint(new(255, 255, 255)),
                LabelsPaint = new SolidColorPaint(new(255, 255, 255)),
            }
        });
    }
}
