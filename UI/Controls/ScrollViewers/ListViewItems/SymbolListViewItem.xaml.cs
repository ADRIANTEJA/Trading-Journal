using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace UI.Controls.ScrollViewers.ListViewItems;
/// <summary>
/// Interaction logic for SymbolListViewItem.xaml
/// </summary>
public partial class SymbolListViewItem : Border
{
    private readonly IEventAggregator _eventAggregator;

    public SymbolListViewItem()
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
    }

    private void OnDeleteSymbolButtonClickHandler(object sender, RoutedEventArgs e)
    {
        var contextSymbol = (Symbol)DataContext;

        _eventAggregator.GetEvent<DeleteSymbolClickEvent>().Publish(contextSymbol.Id);
    }
}
