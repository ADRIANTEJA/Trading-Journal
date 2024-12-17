using LiveCharts.Wpf;
using MainModule.ViewModels;
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

    private void OnStrategyViewLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)DataContext;
        dataContext.LoadStrategiesCommand.Execute(null);
        
        if (strategies_listview.Items.Count > 0) strategies_listview.SelectedValue = strategies_listview.Items[0];
    }
    //remenber to delete if unused
    private void StrategySelectionChangedHandler(object sender, SelectionChangedEventArgs e)
    {
    }
}
