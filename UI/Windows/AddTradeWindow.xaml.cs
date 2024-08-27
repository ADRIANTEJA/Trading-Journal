using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Events;
using UI.Settings;

namespace UI.Windows;

public partial class AddTradeWindow : Window
{
    public AddTradeWindow(IEventAggregator eventAggregator)
    {
        InitializeComponent();
        eventAggregator.GetEvent<OnUILanguageChangedEvent>().Subscribe(ChangeWindowCultureHandler);
        eventAggregator.GetEvent<OnUILanguageChangedEvent>().Publish();
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void ValidateNumericValueHandler(object sender, TextChangedEventArgs e)
    {
        var textBoxRef = (TextBox)sender;

        if (string.IsNullOrEmpty(textBoxRef.Text)) return;
        if (!MiscFunctions.CheckInputIsNumeric(textBoxRef.Text)) textBoxRef.Text = "0";

        textBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void ChangeWindowCultureHandler() => UISettings.ApplyElementCultureSettings(this);

    private void ShowAssetTypesHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["show_asset_types_storyboard"];
        sBoard.Begin();
    }

    private void HideAssetTypesHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["hide_asset_types_storyboard"];
        sBoard.Begin();
    }

    private void ShowSymbolsHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["show_symbols_storyboard"];
        sBoard.Begin();
    }

    private void HideSymbolsHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["hide_symbols_storyboard"];
        sBoard.Begin();
    }

    private void OnAddTradeWindowLoaded(object sender, RoutedEventArgs e)
    {
        var symbolListViewRef = (ListView)symbol_selector.FindName("options_list_view");

        var itemButtonRef = (Button)symbolListViewRef.ItemTemplate.FindName("item_button", symbolListViewRef);
        itemButtonRef.Click += UpdateSymbolListSelectedItem;// LEFT HERE
    }

    private void UpdateSymbolListSelectedItem(object sender, RoutedEventArgs e)
    {
        var buttonRef = (Button)sender;

        var listViewRef = (ListView)symbol_selector.FindName("options_list_view");
        listViewRef.SelectedValue = buttonRef.Content;
    }

    private void OnSymbolSelectorLoaded(object sender, RoutedEventArgs e)
    {

    }
}
