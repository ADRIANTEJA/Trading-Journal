using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace UI.Controls.ScrollViewers.Custom;
/// <summary>
/// Interaction logic for SymbolFilterControl.xaml
/// </summary>
public partial class SymbolFilterControl : Border
{
    public SymbolFilterControl()
    {
        InitializeComponent();
    }

    private void ShowSymbolCategoryHandler(object sender, RoutedEventArgs e)
    {
        var sBoard = (Storyboard)Resources["expand_symbol_category_storyboard"];
        sBoard.Begin();
    }
}
