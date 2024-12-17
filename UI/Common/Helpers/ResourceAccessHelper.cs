using System.Windows;
using System.Windows.Media;

namespace UI.Common.Helpers;
/// <summary>
/// A helper class to prevent the extensive use of strings to access resources
/// in code behind
/// </summary>
public static class ResourceAccessHelper
{
    private static readonly object strategyPerformanceLabelFormaterRef =
        Application.Current.FindResource("strategy_performance_label_formater");

    public static object StrategyPerformanceLabelFormaterRef => strategyPerformanceLabelFormaterRef;

    private static readonly SolidColorBrush greenBrushRef = 
		(SolidColorBrush)Application.Current.FindResource("green_brush");

	public static SolidColorBrush GreenBrushRef => greenBrushRef;

	private static readonly SolidColorBrush salmonBrushRef =
		(SolidColorBrush)Application.Current.FindResource("salmon_brush");

	public static SolidColorBrush SalmonBrushRef => salmonBrushRef;

	private static readonly SolidColorBrush errorRedBrush = 
		(SolidColorBrush)Application.Current.FindResource("error_red_brush");

    public static SolidColorBrush ErrorRedBrush => errorRedBrush;

    private static readonly SolidColorBrush warningYellowBrush =
        (SolidColorBrush)Application.Current.FindResource("warning_yellow_brush");

    public static SolidColorBrush WarningYellowBrush => warningYellowBrush;

    private static double fontSize = (double)Application.Current.FindResource("font_size");

    public static double FontSize => fontSize;

    private static readonly string themeChartAxisPaintKey = "theme_chart_axis_paint";

    public static string ThemeChartAxisPaintKey => themeChartAxisPaintKey;

    private static readonly string themePlaceHolderBrushKey = "theme_place_holder_brush";

    public static string ThemePlaceHolderBrushKey => themePlaceHolderBrushKey;

	private static readonly string themeBackgroundInverseBrushKey = "theme_background_inverse_brush";

    public static string ThemeBackgroundInverseBrushKey => themeBackgroundInverseBrushKey;

    private static readonly FrameworkElement grabCursorDummy = 
		(FrameworkElement)Application.Current.FindResource("grab_cursor_dummy");

	public static FrameworkElement GrabCursorDummy => grabCursorDummy;
    
    private static readonly FrameworkElement grabbingCursorDummy =
        (FrameworkElement)Application.Current.FindResource("grabbing_cursor_dummy");

    public static FrameworkElement GrabbingCursorDummy => grabbingCursorDummy;

    private static readonly string duplicatedAccountErrorMessageKey = "add_account_window_duplicated_account_error_message";

    public static string DuplicatedAccountErrorMessageKey => duplicatedAccountErrorMessageKey;

    private static readonly string addSymbolWindowAssetTypeErrorMessageKey = "add_symbol_window_asset_type_error";

    public static string AddSymbolWindowAssetTypeErrorMessageKey => addSymbolWindowAssetTypeErrorMessageKey;

    private static readonly string addTradeWindowAssetTypeSelectorHeaderKey = "add_trade_window_asset_type_selector_header";

    public static string AddTradeWindowAssetTypeSelectorHeaderKey => addTradeWindowAssetTypeSelectorHeaderKey;

    private static readonly string duplicatedSymbolNameErrorKey = "add_symbol_window_duplicated_symbol_name_error";

    public static string DuplicatedSymbolNameErrorKey => duplicatedSymbolNameErrorKey;

    private static readonly string duplicatedStrategyNameErrorKey = "add_strategy_window_duplicated_strategy_name_error";

    public static string DuplicatedStrategyNameErrorKey => duplicatedStrategyNameErrorKey;

    private static readonly string missingSymbolErrorKey = "add_trade_window_missing_symbol_error";

    public static string MissingSymbolErrorKey => missingSymbolErrorKey;
}
