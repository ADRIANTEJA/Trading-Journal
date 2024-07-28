using System.Windows;
using System.Windows.Media;

namespace UI.Common.Helpers;
/// <summary>
/// A helper class to prevent the extensive use of strings to access resources
/// in code behind
/// </summary>
public static class ResourceAccessHelper
{
	private static readonly SolidColorBrush greenBrushRef = 
		(SolidColorBrush)Application.Current.FindResource("green_brush");

	public static SolidColorBrush GreenBrushRef => greenBrushRef;

	private static readonly SolidColorBrush salmonBrushRef =
		(SolidColorBrush)Application.Current.FindResource("salmon_brush");

	public static SolidColorBrush SalmonBrushRef => salmonBrushRef;

	private static readonly SolidColorBrush errorRedBrush = 
		(SolidColorBrush)Application.Current.FindResource("error_red_brush");

    public static SolidColorBrush ErrorRedBrush => errorRedBrush;

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
}
