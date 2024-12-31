using API.Events;
using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Helpers;
using UI.Common.Utils;
using UI.Controls.TextBoxes;

namespace UI.Windows;
/// <summary>
/// Interaction logic for AddAccountWindow.xaml
/// </summary>
public partial class AddAccountWindow : Window
{
    public AddAccountWindow(IEventAggregator eventAggregator)
    {
        InitializeComponent();
        eventAggregator.GetEvent<CreateAccountEvent>().Subscribe(AccountCreationHandler);
    }

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

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

    private bool IsInputValid()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(account_name_field.Text))
        {
            account_name_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(account_initial_balance_field.Text))
        {
            account_initial_balance_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }
        return isValid;
    }

    private void AccountCreationHandler(bool success)
    {
        if (success) Close();
        else
        {
            duplicated_account_error_label.Text =
            Application.Current.FindResource(ResourceAccessHelper.DuplicatedAccountErrorMessageKey).ToString();
            account_name_field.Tag = ResourceAccessHelper.ErrorRedBrush;
        }
    }

    private void CreateAccountClickHandler(object sender, RoutedEventArgs e)
    {
        if (!IsInputValid())
        {
            create_account_button.Focus();
            return;
        }
        var dataContext = (AccountViewModel)DataContext;
        dataContext.AddAccountCommand.Execute(null);
    }

    private void OnNameFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        if(!string.IsNullOrEmpty(account_name_field.Text))
            account_name_field.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (PlaceHolderTextBox)sender;

        switch (e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "account_name_field"
                    && !string.IsNullOrEmpty(account_name_field.Text)) Keyboard.Focus(account_initial_balance_field);
                break;
            case Key.Up:
                if (senderRef.Name == "account_initial_balance_field") Keyboard.Focus(account_name_field);
                break;
            case Key.Down:
                if (senderRef.Name == "account_name_field") Keyboard.Focus(account_initial_balance_field);
                break;
        }
    }
}
