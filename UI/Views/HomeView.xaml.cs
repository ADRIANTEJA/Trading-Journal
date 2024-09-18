using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace UI.Views;
/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void OnHomeViewLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (HomeViewModel)DataContext;
        dataContext.AccountViewModel.LoadDailyPerformanceCommand.Execute(null);
        dataContext.AccountViewModel.LoadAccountsCommand.Execute(null);
        dataContext.LoadTradesCommand.Execute(null);
        dataContext.SymbolViewModel.LoadSymbolsCommand.Execute(null);
    }

    private void ShowSymbolCategoryHandler(object sender, RoutedEventArgs e)
    {
        if (expand_symbol_category_button.Tag.ToString() == "1")
        {
            expand_symbol_category_button.SetResourceReference(StyleProperty, "symbol_category_button_collapse_style");
            expand_symbol_category_button.Tag = "0";
            var sBoard = (Storyboard)Resources["show_symbol_categories_storyboard"];
            sBoard.Begin();
        }
        else
        {
            expand_symbol_category_button.SetResourceReference(StyleProperty, "symbol_category_button_expand_style");
            expand_symbol_category_button.Tag = "1";
            var sBoard = (Storyboard)Resources["hide_symbol_categories_storyboard"];
            sBoard.Begin();
        }
    }

    private void ShowSymbolCategoryFiltersHandller(object sender, MouseEventArgs e)
    {
        var senderRef = (Border)sender;

        Storyboard sBoard = new();

        switch(senderRef.Name)
        {
            case "crypto_expander":
                sBoard = (Storyboard)Resources["expand_crypto_filter_storyboard"];
                sBoard.Begin();
                break;
            case "forex_expander":
                sBoard = (Storyboard)Resources["expand_forex_filter_storyboard"];
                sBoard.Begin();
                break;
            case "indices_expander":
                sBoard = (Storyboard)Resources["expand_indices_filter_storyboard"];
                sBoard.Begin();
                break;
            case "etfs_expander":
                sBoard = (Storyboard)Resources["expand_etfs_filter_storyboard"];
                sBoard.Begin();
                break;
            case "stocks_expander":
                sBoard = (Storyboard)Resources["expand_stocks_filter_storyboard"];
                sBoard.Begin();
                break;
            case "commodities_expander":
                sBoard = (Storyboard)Resources["expand_commodities_filter_storyboard"];
                sBoard.Begin();
                break;
        }
    }

    private void HideSymbolCategoryFiltersHandller(object sender, MouseEventArgs e)
    {
        var senderRef = (Border)sender;

        Storyboard sBoard = new();

        switch (senderRef.Name)
        {
            case "crypto_expander":
                sBoard = (Storyboard)Resources["collapse_crypto_filter_storyboard"];
                sBoard.Begin();
                break;
            case "forex_expander":
                sBoard = (Storyboard)Resources["collapse_forex_filter_storyboard"];
                sBoard.Begin();
                break;
            case "indices_expander":
                sBoard = (Storyboard)Resources["collapse_indices_filter_storyboard"];
                sBoard.Begin();
                break;
            case "etfs_expander":
                sBoard = (Storyboard)Resources["collapse_etfs_filter_storyboard"];
                sBoard.Begin();
                break;
            case "stocks_expander":
                sBoard = (Storyboard)Resources["collapse_stocks_filter_storyboard"];
                sBoard.Begin();
                break;
            case "commodities_expander":
                sBoard = (Storyboard)Resources["collapse_commodities_filter_storyboard"];
                sBoard.Begin();
                break;
        }
    }
}
