using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace UI.Controls.ScrollViewers.Custom;
/// <summary>
/// Interaction logic for SymbolSelector.xaml
/// </summary>
public partial class SymbolSelector : Border
{
    public SymbolSelector()
    {
        InitializeComponent();
    }

    private void OnSymbolSelectorLoaded(object sender, RoutedEventArgs e)
    {
        if (options_listview.Items.Count > 0) options_listview.SelectedValue = options_listview.Items[0];
    }

    private void OnOptionsListViewLoaded(object sender, RoutedEventArgs e)
    {
        ((INotifyCollectionChanged)options_listview.ItemsSource).CollectionChanged += OnSymbolsItemSourceChangedHandler;
    }

    private void OnSymbolsItemSourceChangedHandler(Object sender, NotifyCollectionChangedEventArgs e)
    {
        if (options_listview.Items.Count > 0) options_listview.SelectedValue = options_listview.Items[0];
    }

    private void SymbolSelectorUnloadedHandler(object sender, RoutedEventArgs e)
    {
        ((INotifyCollectionChanged)options_listview.ItemsSource).CollectionChanged -= OnSymbolsItemSourceChangedHandler;
    }
}
