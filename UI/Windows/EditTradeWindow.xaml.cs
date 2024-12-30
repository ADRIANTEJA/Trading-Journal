using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Helpers;
using UI.Common.Utils;

namespace UI.Windows;
/// <summary>
/// Interaction logic for EditTradeWindow.xaml
/// </summary>
public partial class EditTradeWindow : Window
{
    public EditTradeWindow()
    {
        InitializeComponent();
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
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

    private void ValidateNumericValueHandler(object sender, TextChangedEventArgs e)
    {
        var textBoxRef = (TextBox)sender;

        if (string.IsNullOrEmpty(textBoxRef.Text)) return;
        if (!MiscFunctions.CheckInputIsNumeric(textBoxRef.Text)) textBoxRef.Text = "0";

        textBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void OnOpenDateFieldLoaded(object sender, RoutedEventArgs e)
    {
        var openDateTextBoxRef = (TextBox)open_date_field.Template.FindName("PART_TextBox", open_date_field);
        openDateTextBoxRef.TextChanged += OpenDateFieldChangedHandler;
    }

    private void OpenDateFiledUnloadedHandler(object sender, RoutedEventArgs e)
    {
        var openDateTextBoxRef = (TextBox)open_date_field.Template.FindName("PART_TextBox", open_date_field);
        openDateTextBoxRef.TextChanged -= OpenDateFieldChangedHandler;
    }

    private void OnCloseDateFieldLoaded(object sender, RoutedEventArgs e)
    {
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);
        closeDateTextBoxRef.TextChanged += CloseDateFieldChangedHandler;
    }

    private void CloseDateFieldUnloadedHandler(object sender, RoutedEventArgs e)
    {
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);
        closeDateTextBoxRef.TextChanged -= CloseDateFieldChangedHandler;
    }

    private void OpenDateFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        var openDateTextBoxRef = (TextBox)open_date_field.Template.FindName("PART_TextBox", open_date_field);

        if (!string.IsNullOrEmpty(openDateTextBoxRef.Text))
            openDateTextBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);

        try { open_date_field.SelectedDate = DateTime.ParseExact(openDateTextBoxRef.Text, "dd/MM/yyyy hh.mm tt", null); }
        catch (FormatException) { }
    }

    private void CloseDateFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        var closeDateTextBoxRef = (TextBox)close_date_field.Template.FindName("PART_TextBox", close_date_field);

        if (!string.IsNullOrEmpty(closeDateTextBoxRef.Text))
            closeDateTextBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);

        try { close_date_field.SelectedDate = DateTime.ParseExact(closeDateTextBoxRef.Text, "dd/MM/yyyy hh.mm tt", null); }
        catch (FormatException) { }
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

    private bool IsInputValid()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(volume_field.Text) || double.Parse(volume_field.Text) == 0)
        {
            volume_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(open_price_field.Text) || double.Parse(open_price_field.Text) == 0)
        {
            open_price_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (double.Parse(close_price_field.Text) == 0)
        {
            close_price_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        return isValid;
    }

    private void EditTradeClickHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (HomeViewModel)DataContext;
        var leverageSliderRef = (Slider)trade_leverage_slider.FindName("leverage_slider");

        int failSafeLeverage = 0;

        //this is done because of the slider having defualt value 0
        if(leverageSliderRef.Value == 0) failSafeLeverage = 1;
        else failSafeLeverage = (int)leverageSliderRef.Value;

        if (!IsInputValid())
        {
            edit_trade_button.Focus();
            return;
        }
        if (!AreDatesValid())
        {
            edit_trade_button.Focus();
            return;
        }

        var updatedTrade = dataContext.SelectedTrade;
        var tradeClosed = false;

        updatedTrade.Volume = double.Parse(volume_field.Text);
        updatedTrade.OpenPrice = double.Parse(open_price_field.Text);
        updatedTrade.Leverage = failSafeLeverage;

        if (!string.IsNullOrEmpty(close_price_field.Text))
            updatedTrade.ClosePrice = double.Parse(close_price_field.Text);
        if (!string.IsNullOrEmpty(swap_field.Text))
            updatedTrade.Swap = double.Parse(swap_field.Text);
        if (!string.IsNullOrEmpty(spread_field.Text))
            updatedTrade.Spread = double.Parse(spread_field.Text);
        if (!string.IsNullOrEmpty(spread_field.Text))
            updatedTrade.Commission = double.Parse(spread_field.Text);
        if (!string.IsNullOrEmpty(other_costs_field.Text))
            updatedTrade.OtherCosts = double.Parse(other_costs_field.Text);
        if (!string.IsNullOrEmpty(take_profit_field.Text))
            updatedTrade.TakeProfit = double.Parse(take_profit_field.Text);
        if (!string.IsNullOrEmpty(stop_loss_field.Text))
            updatedTrade.StopLoss = double.Parse(stop_loss_field.Text);

        updatedTrade.OpenDate = open_date_field.SelectedDate!.Value.Ticks;

        if (close_date_field.SelectedDate != null)
            updatedTrade.CloseDate = close_date_field.SelectedDate.Value.Ticks;

        if (updatedTrade.CloseDate != null && updatedTrade.ClosePrice > 0)
            updatedTrade.IsOpen = 0;

        if (dataContext.SelectedTrade.IsOpen == 1 
            && updatedTrade.IsOpen == 0) tradeClosed = true;

        dataContext.UpdateTradeCommand.Execute(updatedTrade, tradeClosed);
        Close();
    }
}
