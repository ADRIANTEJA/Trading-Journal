using System.Windows;
using System.Windows.Controls;

namespace UI.Controls.ScrollViewers.Custom;
/// <summary>
/// Interaction logic for AssetTypeSelector.xaml
/// </summary>
public partial class AssetTypeSelector : Border
{
    public AssetTypeSelector()
    {
        InitializeComponent();
    }

    private void OnOptionsListviewLoaded(object sender, RoutedEventArgs e)
    {
        options_listview.SelectedValue = options_listview.Items[0];
    }
}
