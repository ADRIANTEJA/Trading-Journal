using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace UI.Controls.ScrollViewers.Custom;
/// <summary>
/// Interaction logic for StrategySelector.xaml
/// </summary>
public partial class StrategySelector : Border
{
    public StrategySelector()
    {
        InitializeComponent();
    }

    private void OnStrategySelectorLoaded(object sender, RoutedEventArgs e)
    {
        if (options_listview.Items.Count > 0) options_listview.SelectedValue = null;
    }

    private void OnOptionsListViewLoaded(object sender, RoutedEventArgs e)
    {
        ((INotifyCollectionChanged)options_listview.ItemsSource).CollectionChanged += OnSymbolsItemSourceChangedHandler;
    }

    private void OnSymbolsItemSourceChangedHandler(Object sender, NotifyCollectionChangedEventArgs e)
    {
        if (options_listview.Items.Count > 0) options_listview.SelectedValue = options_listview.Items[0];
    }

    private void StrategySelectorUnloadedHandler(object sender, RoutedEventArgs e)
    {
        ((INotifyCollectionChanged)options_listview.ItemsSource).CollectionChanged -= OnSymbolsItemSourceChangedHandler;
    }
}
