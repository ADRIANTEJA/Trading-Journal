using API.Events;
using MainModule.DataModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Controls.ScrollViewers.ListViewItems;
/// <summary>
/// Interaction logic for StrategyListViewItem.xaml
/// </summary>
public partial class StrategyListViewItem : Border
{
    private readonly IEventAggregator _eventAggregator;

    public StrategyListViewItem()
    {
        InitializeComponent();

        _eventAggregator = App.AppHost!.Services.GetRequiredService<IEventAggregator>();
    }

    private void OpenAnalysisNotesClickHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<LoadAnalysisNotesEvent>().Publish();

    private void OpenEditStrategyWindowHandler(object sender, RoutedEventArgs e) =>
        _eventAggregator.GetEvent<EditStrategyEvent>().Publish();

    private void UpdateContextStrategyHandler(object sender, MouseEventArgs e)
    {
        var contextStrategy = (Strategy)DataContext;
        _eventAggregator.GetEvent<SelectedStrategyItemChangedEvent>().Publish(contextStrategy);
    }
}
