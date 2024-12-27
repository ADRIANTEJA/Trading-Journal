using MainModule.ViewModels;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UI.Common.Converters;

namespace UI.Views;

public partial class StrategyView : UserControl
{
    private object dataContextRef;

    public StrategyView()
    {
        InitializeComponent();
    }

    private void OnStrategyViewLoaded(object sender, RoutedEventArgs e)
    {
        dataContextRef = DataContext;
    }

    private void OnStrategiesListLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)dataContextRef;
        dataContext.LoadStrategiesCommand.Execute(null);
    }

    private void OnDailyLossConstraintVWarningLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)dataContextRef;

        var listView = (ListView)daily_loss_constraint_warning.FindName("dates_listview");

        var binding = new Binding(nameof(dataContext.SelectedStrategy))
        {
            Converter = new DailyLossConstraintDatesConverter()
        };
        listView.SetBinding(ListView.ItemsSourceProperty, binding);
    }

    private void OnStrategyViewLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)DataContext;
        dataContext.Strategies.CollectionChanged += UpdateAddStrategyButtonVisibilityHandler;
    }

    private void UpdateAddStrategyButtonVisibilityHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
        var dataContext = (StrategyViewModel)dataContextRef;

        if (dataContext.Strategies.Count >= 10) add_strategy_button.Visibility = Visibility.Hidden;
        else add_strategy_button.Visibility = Visibility.Visible;
    }

    private void OnStrategyViewUnloadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)dataContextRef;
        dataContext.Strategies.CollectionChanged -= UpdateAddStrategyButtonVisibilityHandler;
    }
}
