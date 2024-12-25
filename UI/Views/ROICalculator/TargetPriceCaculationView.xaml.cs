using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Controls.TextBoxes;

namespace UI.Views.ROICalculator;
/// <summary>
/// Interaction logic for TargetPriceCaculationView.xaml
/// </summary>
public partial class TargetPriceCaculationView : Page
{
    private bool isLongTrade = true;

    public TargetPriceCaculationView()
    {
        InitializeComponent();
    }

    private void OnViewLoaderHandler(object sender, RoutedEventArgs e)
    {
        long_button.Background = ResourceAccessHelper.GreenBrushRef;
    }

    private void LongOperationSelectionHandler(object sender, RoutedEventArgs e)
    {
        long_button.Background = ResourceAccessHelper.GreenBrushRef;
        short_button.Background = null;
        isLongTrade = true;
    }

    private void ShortOperationSelectionHandler(object sender, RoutedEventArgs e)
    {
        short_button.Background = ResourceAccessHelper.SalmonBrushRef;
        long_button.Background = null;
        isLongTrade = false;
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (PlaceHolderTextBox)sender;

        switch (e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "amount_field"
                    && !string.IsNullOrEmpty(amount_field.Text)) Keyboard.Focus(open_price_field);

                if (senderRef.Name == "open_price_field"
                    && !string.IsNullOrEmpty(open_price_field.Text)) Keyboard.Focus(ROI_field);
                break;
            case Key.Up:
                if (senderRef.Name == "ROI_field") Keyboard.Focus(open_price_field);
                if (senderRef.Name == "open_price_field") Keyboard.Focus(amount_field);
                break;
            case Key.Down:
                if (senderRef.Name == "amount_field") Keyboard.Focus(open_price_field);
                if (senderRef.Name == "open_price_field") Keyboard.Focus(ROI_field);
                break;
        }
    }

    private void CaculateTargetPriceHandler(object sender, RoutedEventArgs e)
    {
        if (!IsInputValid())
        {
            calculate_button.Focus();
            return;
        }

        var leverageSliderRef = (Slider)leverage_slider.FindName("leverage_slider");

        double amountToInvest = double.Parse(amount_field.Text);
        double openPrice = double.Parse(open_price_field.Text);
        double TargetProfit = amountToInvest * double.Parse(ROI_field.Text) / 100;
        double volume = amountToInvest * leverageSliderRef.Value / openPrice;

        double targetPrice = 0;

        switch (isLongTrade)
        {
            case true:
                targetPrice = (TargetProfit * openPrice + volume) / volume;
                break;
            case false:
                targetPrice = openPrice / volume * (volume - TargetProfit);
                break;
        }

        if (targetPrice > 0) target_price_field.Text = "$ " + Math.Round(targetPrice, 4);
        else target_price_field.Text = "$ 0";

        initial_margin_label.Visibility = Visibility.Visible;
        target_price_label.Visibility = Visibility.Visible;
        initial_margin_field.Text = "$ " + Math.Round(amountToInvest / leverageSliderRef.Value, 4);      
    }

    private bool IsInputValid()
    {
        bool isValid = true;

        if (!MiscFunctions.CheckInputIsNumeric(amount_field.Text)
            || string.IsNullOrEmpty(amount_field.Text)
            || double.Parse(amount_field.Text) == 0)
        {
            amount_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (!MiscFunctions.CheckInputIsNumeric(open_price_field.Text)
            || string.IsNullOrEmpty(open_price_field.Text)
            || double.Parse(open_price_field.Text) == 0)
        {
            open_price_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (!MiscFunctions.CheckInputIsNumeric(ROI_field.Text)
            || string.IsNullOrEmpty(ROI_field.Text)
            || double.Parse(ROI_field.Text) == 0)
        {
            ROI_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }
        return isValid;
    }

    private void ValidateNumericValueHandler(object sender, TextChangedEventArgs e)
    {
        var textBoxRef = (TextBox)sender;

        if (string.IsNullOrEmpty(textBoxRef.Text)) return;
        if (!MiscFunctions.CheckInputIsNumeric(textBoxRef.Text)) textBoxRef.Text = "0";

        textBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }
}
