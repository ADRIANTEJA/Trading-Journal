using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Events;
using UI.Windows;

namespace UI.Controls.ScrollViewers.ScrollableGridViewItems;
/// <summary>
/// Interaction logic for TradeImageContainer.xaml
/// </summary>
public partial class TradeImageContainer : Border
{
    private readonly IEventAggregator _eventAggregator;

    public TradeImageContainer()
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
    }

    private void DeleteTradeImageHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (TradeImageViewModel)DataContext;
        dataContext.DeleteTradeImageCommand.Execute((int)Tag);

        _eventAggregator.GetEvent<OnTradeImageDeletedEvent>().Publish();
    }

    private void ShowTradeImageHandler(object sender, MouseButtonEventArgs e)
    {
        ShowImageWindow tradeImageWindow = new(image_control.Source);
        tradeImageWindow.Show();
    }
}
