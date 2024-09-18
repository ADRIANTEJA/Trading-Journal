using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Controls.TextBoxes;

namespace UI.Views.ROICalculator;
/// <summary>
/// Interaction logic for ROICalculationView.xaml
/// </summary>
public partial class ROICalculationView : Page
{
    private bool isLongTrade = true;

    public ROICalculationView()
    {
        InitializeComponent();
    }

    private void OnViewLoaderHandler(object sender, RoutedEventArgs e) => long_button.Background = ResourceAccessHelper.GreenBrushRef;

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

    private void ValidateNumericValueHandler(object sender, TextChangedEventArgs e)
    {
        var textBoxRef = (TextBox)sender;

        if (string.IsNullOrEmpty(textBoxRef.Text)) return;
        if (!MiscFunctions.CheckInputIsNumeric(textBoxRef.Text)) textBoxRef.Text = "0";

        textBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void CaculateROIHandler(object sender, RoutedEventArgs e)
    {
        if (!IsInputValid())
        {
            calculate_button.Focus();
            return;
        }

        var leverageSliderRef = (Slider)leverage_slider.FindName("leverage_slider");

        double amountToInvest = double.Parse(amount_field.Text);
        double openPrice = double.Parse(open_price_field.Text);
        double closePrice = double.Parse(close_price_field.Text);

        double profit = amountToInvest / openPrice * (closePrice - openPrice);

        double ROI = profit / amountToInvest * 100;

        if (!isLongTrade)
        {
            ROI *= -1; 
            profit *= -1;
        }

        if (profit > 0)
        {
            profit_field.Foreground = ResourceAccessHelper.GreenBrushRef;
            ROI_field.Foreground = ResourceAccessHelper.GreenBrushRef;
            profit_field.Text = "$ +" + Math.Round(profit, 4);
            ROI_field.Text = "$ +" + Math.Round(ROI, 4) + " %";
        }
        else
        {
            profit_field.Foreground = ResourceAccessHelper.SalmonBrushRef;
            ROI_field.Foreground = ResourceAccessHelper.SalmonBrushRef;
            profit_field.Text = "$ " + Math.Round(profit, 4);
            ROI_field.Text = "$ " + Math.Round(ROI, 4) + " %";
        }

        initial_margin_label.Visibility = Visibility.Visible;
        initial_margin_field.Text = "$ " + Math.Round(amountToInvest / leverageSliderRef.Value, 4);
    }

    private bool IsInputValid()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(amount_field.Text))
        { 
            amount_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(open_price_field.Text)) 
        {
            open_price_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(close_price_field.Text))
        {
            close_price_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }
        return isValid;
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (PlaceHolderTextBox)sender;

        switch(e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "amount_field"
                    && !string.IsNullOrEmpty(amount_field.Text)) Keyboard.Focus(open_price_field);

                if (senderRef.Name == "open_price_field"
                    && !string.IsNullOrEmpty(open_price_field.Text)) Keyboard.Focus(close_price_field);
                break;
            case Key.Up:
                if (senderRef.Name == "close_price_field") Keyboard.Focus(open_price_field);
                if (senderRef.Name == "open_price_field") Keyboard.Focus(amount_field);
                break;
            case Key.Down:
                if (senderRef.Name == "amount_field") Keyboard.Focus(open_price_field);
                if (senderRef.Name == "open_price_field") Keyboard.Focus(close_price_field);
                break;
        }
    }
}
