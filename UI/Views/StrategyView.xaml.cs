using API.Events;
using MainModule.DataAccess;
using MainModule.DataModel;
using MainModule.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UI.Common.Converters;

namespace UI.Views;

public partial class StrategyView : UserControl
{
    public StrategyView()
    {
        InitializeComponent();
    }

    private void OnStrategiesListLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)DataContext;
        dataContext.LoadStrategiesCommand.Execute(null);
    }

    private void OnDailyLossConstraintVWarningLoaded(object sender, RoutedEventArgs e)
    {
        var listView = (ListView)daily_loss_constraint_warning.FindName("dates_listview");

        var binding = new Binding("SelectedStrategy")
        {
            Converter = new DailyLossConstraintDatesConverter()
        };
        listView.SetBinding(ListView.ItemsSourceProperty, binding);
    }
}
