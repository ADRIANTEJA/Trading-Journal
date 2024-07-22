using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Windows;

namespace UI.Common.Resources.Dictionaries;

public partial class DarkThemeDictionary : ResourceDictionary
{
    public DarkThemeDictionary()
    {
        Add("theme_line_chart_xaxes_style", new Axis[]
        {   
            new DateTimeAxis(TimeSpan.FromDays(1), (DateTime date) => date.ToString("dd/mm/yyyy")) 
            {
                NameTextSize = 16,
                NamePaint = new SolidColorPaint(new(255, 255, 255)),
                LabelsPaint = new SolidColorPaint(new(255, 255, 255))
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
                MinLimit = -100
            }
        });
    }
}
