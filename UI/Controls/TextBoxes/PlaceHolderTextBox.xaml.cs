using System.Windows;
using System.Windows.Controls;

namespace UI.Controls.TextBoxes;

public partial class PlaceHolderTextBox : TextBox
{
    public string PlaceHolder
    {
        get { return (string)GetValue(PlaceHolderProperty); }
        set { SetValue(PlaceHolderProperty, value); }
    }

    public static readonly DependencyProperty PlaceHolderProperty =
        DependencyProperty.Register("PlaceHolder", typeof(string),
                                    typeof(PlaceHolderTextBox),
                                    new PropertyMetadata(string.Empty));

    public bool IsEmpty
    {
        get { return (bool)GetValue(IsEmptyProperty); }
        private set { SetValue(IsEmptyPropertyKey, value); }
    }

    private static readonly DependencyPropertyKey IsEmptyPropertyKey =
        DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool),
                                            typeof(PlaceHolderTextBox),
                                            new PropertyMetadata(true));

    public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

    static PlaceHolderTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceHolderTextBox), 
                                                 new FrameworkPropertyMetadata(typeof(PlaceHolderTextBox)));
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        IsEmpty = string.IsNullOrEmpty(Text);
    }
}
