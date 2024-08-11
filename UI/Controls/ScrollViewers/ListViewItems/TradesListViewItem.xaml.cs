using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Controls.ScrollViewers.ListViewItems;
/// <summary>
/// Interaction logic for TradesListViewItem.xaml
/// </summary>
public partial class TradesListViewItem : Border
{
    private IEventAggregator _eventAggregator;

    public TradesListViewItem()
    {
        InitializeComponent();
        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
    }

    private void OpenTradeImagesWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<OnLoadTradeImagesClickEvent>().Publish();

    private void UpdateContextTradeHandler(object sender, MouseEventArgs e)
    {
        var contextTrade = (Trade)DataContext;
        _eventAggregator.GetEvent<OnSelectedTradeItemChangedEvent>().Publish(contextTrade);
    }

    private void OpenTradeNotesWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<OnLoadTradeNotesClickEvent>().Publish();

    private void OpenTradeMistakesWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<OnLoadTradeMistakesClickEvent>().Publish();
}
