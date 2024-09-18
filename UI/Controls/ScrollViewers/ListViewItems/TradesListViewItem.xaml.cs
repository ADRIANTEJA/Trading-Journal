using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Helpers;

namespace UI.Controls.ScrollViewers.ListViewItems;
/// <summary>
/// Interaction logic for TradesListViewItem.xaml
/// </summary>
public partial class TradesListViewItem : Border
{
    private readonly IEventAggregator _eventAggregator;

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

    private void OpenTradeCostsWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<OnLoadTradeCostsClickEvent>().Publish();

    private void OnTradeStatusTextBlockLoaded(object sender, RoutedEventArgs e)
    {
        var contextTrade = (Trade)DataContext;

        if (contextTrade.IsOpen == 1) trade_status_textblock.Text = "OPEN";
        else
        {
            switch (contextTrade.IsLong)
            {
                case 1:
                    if (contextTrade.ClosePrice >= contextTrade.OpenPrice) trade_side_textblock.Text = "WIN";
                    else trade_status_textblock.Text = "LOSS";
                    break;
                case 0:
                    if (contextTrade.ClosePrice <= contextTrade.OpenPrice) trade_status_textblock.Text = "WIN";
                    else trade_status_textblock.Text = "LOSS";
                    break;
            }
        }
    }

    private void OnTradeSideTextBlockLoaded(object sender, RoutedEventArgs e)
    {
        var contextTrade = (Trade)DataContext;

        if (contextTrade.IsLong == 1) trade_side_textblock.Text = "LONG";
        else trade_side_textblock.Text = "SHORT";
    }
}
