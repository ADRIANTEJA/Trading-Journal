using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Events;

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

    private void OnTradeListViewItemLoadedHandler(object sender, RoutedEventArgs e)
    {
        _eventAggregator.GetEvent<SelectAllTradesCheckBoxClickedEvent>().Subscribe(GlobalTradesSelectionChangedHandler);
    }

    private void OpenTradeImagesWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<LoadTradeImagesEvent>().Publish();

    private void UpdateContextTradeHandler(object sender, MouseEventArgs e)
    {
        var contextTrade = (Trade)DataContext;
        _eventAggregator.GetEvent<SelectedTradeItemChangedEvent>().Publish(contextTrade);
    }

    private void OpenTradeNotesWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<LoadTradeNotesEvent>().Publish();

    private void OpenTradeMistakesWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<LoadTradeMistakesEvent>().Publish();

    private void OpenTradeCostsWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<LoadTradeCostsEvent>().Publish();

    private void OpenEditTradeWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<EditTradeEvent>().Publish();

    private void OnTradeStatusTextBlockLoaded(object sender, RoutedEventArgs e)
    {
        var contextTrade = (Trade)DataContext;

        if (contextTrade.IsOpen == 1) trade_status_textblock.Text = "OPEN";
        else
        {
            switch (contextTrade.IsLong)
            {
                case 1:
                    if (contextTrade.ClosePrice >= contextTrade.OpenPrice) trade_status_textblock.Text = "WIN";
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

    private void OnTradeRiskWarningMouseEnterHandler(object sender, MouseEventArgs e)
    {
        trade_risk_constraint_warning.Visibility = Visibility.Visible;
    }

    private void OnTradeRiskWarningMouseLeaveHandler(object sender, MouseEventArgs e)
    {
        if (!trade_risk_constraint_warning.IsMouseOver)
            trade_risk_constraint_warning.Visibility = Visibility.Hidden;
    }

    private void GlobalTradesSelectionChangedHandler(bool isChecked)
    {
        if (isChecked) trade_checkbox.IsChecked = true;
        else trade_checkbox.IsChecked = false;
    }

    private void OnTradeCheckBoxCheckedHandler(object sender, RoutedEventArgs e)
    {
        var contextTrade = (Trade)DataContext;

        _eventAggregator.GetEvent<TradeSelectionChangedEvent>().Publish(new()
        {
            TradeId = contextTrade.Id,
            IsSelected = true
        });
    }

    private void OnTradeCheckBoxUncheckedHandler(object sender, RoutedEventArgs e)
    {
        var contextTrade = (Trade)DataContext;

        _eventAggregator.GetEvent<TradeSelectionChangedEvent>().Publish(new()
        {
            TradeId = contextTrade.Id,
            IsSelected = false
        });
    }
}
