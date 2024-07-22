using LiveChartsCore.SkiaSharpView.WPF;

namespace UI.Controls.Charts.AccountView;
/// <summary>
/// Interaction logic for AccountPerformanceLineChart.xaml
/// </summary>
public partial class AccountPerformanceLineChart : CartesianChart
{
    public AccountPerformanceLineChart()
    {
        InitializeComponent();

        SetResourceReference(XAxesProperty, "theme_line_chart_xaxes_style");

        SetResourceReference(YAxesProperty, "theme_line_chart_yaxes_style");
    }

}
