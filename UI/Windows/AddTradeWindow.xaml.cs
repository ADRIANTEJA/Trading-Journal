using API.Events;
using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Events;
using UI.Settings;

namespace UI.Windows;

public partial class AddTradeWindow : Window
{
    private int isLongTrade = 1;

    public AddTradeWindow(IEventAggregator eventAggregator)
    {
        InitializeComponent();
        eventAggregator.GetEvent<UILanguageChangedEvent>().Subscribe(ChangeWindowCultureHandler);
        eventAggregator.GetEvent<UILanguageChangedEvent>().Publish();
        eventAggregator.GetEvent<CreateTradeEvent>().Subscribe(TradeCreationHandler);
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

    private void ShowStrategiesHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["show_strategies_storyboard"];
        sBoard.Begin();
    }

    private void HideStrategiesHandler(object sender, MouseEventArgs e)
    {
        var sBoard = (Storyboard)Resources["hide_strategies_storyboard"];
        sBoard.Begin();
    }

    private void OnAddTradeWindowLoaded(object sender, RoutedEventArgs e)
    {
        var assetTypeListRef = (ListView)asset_type_selector.FindName("options_listview");
        var binding = new Binding("SelectedValue")
        {
            ElementName = assetTypeListRef.Name,
        };
        selected_asset_textblock.SetBinding(TextBlock.TextProperty, binding);
    }

    private void OnAssetTypeSelectionChangedHandler(object sender, RoutedEventArgs e)
    {
        var optionsListRef = (ListView)asset_type_selector.FindName("options_listview");
        optionsListRef.SelectionChanged += AssetTypeChangedHandler;
    }

    private void AssetTypeChangedHandler(object sender, SelectionChangedEventArgs e)
    {
        var optionsListRef = (ListView)asset_type_selector.FindName("options_listview");

        var dataContext = (HomeViewModel)DataContext;
        dataContext.SymbolViewModel.LoadSymbolsByAssetTypeCommand.Execute(optionsListRef.SelectedValue.ToString());
    }

    private bool IsInputValid()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(volume_field.Text))
        {
            volume_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(open_price_field.Text))
        {
            open_price_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        return isValid;
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (TextBox)sender;

        switch (e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "volume_field"
                    && !string.IsNullOrEmpty(volume_field.Text)) Keyboard.Focus(open_price_field);
                if (senderRef.Name == "open_price_field"
                    && !string.IsNullOrEmpty(open_price_field.Text)) Keyboard.Focus(close_price_field);
                if (senderRef.Name == "close_price_field"
                    && !string.IsNullOrEmpty(close_price_field.Text)) Keyboard.Focus(swap_field);
                if (senderRef.Name == "swap_field"
                    && !string.IsNullOrEmpty(swap_field.Text)) Keyboard.Focus(spread_field);
                if (senderRef.Name == "spread_field"
                    && !string.IsNullOrEmpty(spread_field.Text)) Keyboard.Focus(commissions_field);
                if (senderRef.Name == "commissions_field"
                    && !string.IsNullOrEmpty(commissions_field.Text)) Keyboard.Focus(other_costs_field);
                if (senderRef.Name == "other_costs_field"
                    && !string.IsNullOrEmpty(other_costs_field.Text)) Keyboard.Focus(take_profit_field);
                if (senderRef.Name == "take_profit_field"
                    && !string.IsNullOrEmpty(take_profit_field.Text)) Keyboard.Focus(stop_loss_field);
                if (senderRef.Name == "stop_loss_field"
                    && !string.IsNullOrEmpty(stop_loss_field.Text)) Keyboard.Focus(notes_field);
                if (senderRef.Name == "notes_field"
                    && !string.IsNullOrEmpty(notes_field.Text)) Keyboard.Focus(mistakes_field);
                break;
        }
    }

    private void OnLongButtonLoaded(object sender, RoutedEventArgs e) => long_button.Background = ResourceAccessHelper.GreenBrushRef;

    private void LongOperationSelectionHandler(object sender, RoutedEventArgs e)
    {
        long_button.Background = ResourceAccessHelper.GreenBrushRef;
        short_button.Background = null;
        isLongTrade = 1;
    }

    private void ShortOperationSelectionHandler(object sender, RoutedEventArgs e)
    {
        short_button.Background = ResourceAccessHelper.SalmonBrushRef;
        long_button.Background = null;
        isLongTrade = 0;
    }

    private bool AreDatesValid()
    {
        bool areValid = true;

        var openDateTextBoxRef = (TextBox)open_date_field.Template.FindName("PART_TextBox", open_date_field);
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);

        if (string.IsNullOrEmpty(openDateTextBoxRef.Text))
        {
            openDateTextBoxRef.Tag = ResourceAccessHelper.ErrorRedBrush;
            areValid = false;
        }

        try { long openDate = DateTime.ParseExact(openDateTextBoxRef.Text, "dd/MM/yyyy hh.mm tt", null).Ticks; }
        catch (FormatException)
        {
            openDateTextBoxRef.Tag = ResourceAccessHelper.ErrorRedBrush;
            areValid = false;
        }

        if (!string.IsNullOrEmpty(closeDateTextBoxRef.Text))
        {
            try { long closeDate = DateTime.ParseExact(closeDateTextBoxRef.Text, "dd/MM/yyyy hh.mm tt", null).Ticks; }
            catch (FormatException) 
            {
                closeDateTextBoxRef.Tag = ResourceAccessHelper.ErrorRedBrush;
                areValid = false;
            }
        }

        return areValid;
    }

    private void AddTradeClickHandler(object sender, RoutedEventArgs e)
    {
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);
        var dataContext = (HomeViewModel)DataContext;

        if (!IsInputValid())
        {
            add_trade_button.Focus();
            return;
        }
        if (!AreDatesValid())
        {
            add_trade_button.Focus();
            return;
        }

        if (string.IsNullOrEmpty(closeDateTextBoxRef.Text) || string.IsNullOrEmpty(close_price_field.Text))
        {
            dataContext.AddOpenTradeCommand.Execute(isLongTrade);
        }
        else { dataContext.AddClosedTradeCommand.Execute(isLongTrade); }
    }

    private void TradeCreationHandler(bool success)
    {
        if (success) Close();
        else
        {
            input_error_textblock.Text =
            Application.Current.FindResource(ResourceAccessHelper.MissingSymbolErrorKey).ToString();
            input_error_textblock.Visibility = Visibility.Visible;
        }
    }

    private void OnOpenDateFieldLoaded(object sender, RoutedEventArgs e)
    {
        var openDateTextBoxRef = (TextBox)open_date_field.Template.FindName("PART_TextBox", open_date_field);
        openDateTextBoxRef.TextChanged += OpenDateFieldChangedHandler;
    }

    private void OnCloseDateFieldLoaded(object sender, RoutedEventArgs e)
    {
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);
        closeDateTextBoxRef.TextChanged += CloseDateFieldChangedHandler;
    }

    private void OpenDateFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        var openDateTextBoxRef = (TextBox)open_date_field.Template.FindName("PART_TextBox", open_date_field);

        if (!string.IsNullOrEmpty(openDateTextBoxRef.Text))
            openDateTextBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);

        try { open_date_field.SelectedDate = DateTime.ParseExact(openDateTextBoxRef.Text, "dd/MM/yyyy hh.mm tt", null); }
        catch(FormatException) { }
    }   

    private void CloseDateFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);

        if (!string.IsNullOrEmpty(closeDateTextBoxRef.Text))
            closeDateTextBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);

        try { close_date_field.SelectedDate = DateTime.ParseExact(closeDateTextBoxRef.Text, "dd/MM/yyyy hh.mm tt", null); }
        catch (FormatException) { }
    }

    private void OnLeverageSliderLoaded(object sender, RoutedEventArgs e)
    {
        var leverageSliderRef = (Slider)trade_leverage_slider.FindName("leverage_slider");
        var binding = new Binding("LeverageVM")
        {
            Mode = BindingMode.OneWayToSource
        };

        leverageSliderRef.SetBinding(Slider.ValueProperty, binding);
    }
}
