using System.Windows;
using System.Windows.Controls;
using UI.Common.Helpers;
using UI.Common.Utils;

namespace UI.Controls.TextBoxes;
/// <summary>
/// Interaction logic for NumericUpDownField.xaml
/// </summary>
public partial class NumericUpDownField : Border
{
    public NumericUpDownField()
    {
        InitializeComponent();
    }

    private void ValidateNumericValueHandler(object sender, TextChangedEventArgs e)
    {
        var textBoxRef = (TextBox)sender;

        if (!MiscFunctions.CheckInputIsNumeric(textBoxRef.Text)
            || string.IsNullOrEmpty(textBoxRef.Text)
            || double.Parse(textBoxRef.Text) == 0
            || textBoxRef.Text.Contains('.')
            || textBoxRef.Text[0] == '0')textBoxRef.Text = "1";

        textBoxRef.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void NumericUpDownFieldLoadedHandler(object sender, RoutedEventArgs e)
    {
        divisor_field.Text = "1";
    }

    private void DivisorUpClickHandler(object sender, RoutedEventArgs e)
    {
        divisor_field.Text = (double.Parse(divisor_field.Text) + 1).ToString();
    }

    private void DivisorDownClickHandler(object sender, RoutedEventArgs e)
    {
        divisor_field.Text = (double.Parse(divisor_field.Text) - 1).ToString();
    }
}
