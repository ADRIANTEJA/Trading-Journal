using API.Events;
using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UI.Common.Converters;
using UI.Common.Helpers;
using UI.Common.Utils;

namespace UI.Windows;
/// <summary>
/// Interaction logic for AddStrategyWindow.xaml
/// </summary>
public partial class AddStrategyWindow : Window
{
    public AddStrategyWindow(IEventAggregator eventAggregator)
    {
        InitializeComponent();
        eventAggregator.GetEvent<CreateStrategyEvent>().Subscribe(StrategyCreationHandler);
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void RiskRewardRatioControlLoadedHandler(object sender, RoutedEventArgs e)
    {
        var divisorFieldRef = (TextBox)risk_reward_ratio_control.FindName("divisor_field");

        var binding = new Binding("RiskRewardRatioVM")
        {
            Mode = BindingMode.OneWayToSource,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        divisorFieldRef.SetBinding(TextBox.TextProperty, binding);
    }

    private void StrategyCreationHandler(bool success)
    {
        if (success) Close();
        else input_error_textblock.Visibility = Visibility.Visible;
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

        if (string.IsNullOrEmpty(name_field.Text))
        {
            name_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(max_trade_risk_field.Text) || double.Parse(max_trade_risk_field.Text) == 0)
        {
            max_trade_risk_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(daily_goal_field.Text) || double.Parse(daily_goal_field.Text) == 0)
        {
            daily_goal_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(max_daily_loss_field.Text) || double.Parse(max_daily_loss_field.Text) == 0)
        {
            max_daily_loss_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        return isValid;
    }

    private void AddStrategyClickHandler(object sender, RoutedEventArgs e)
    {
        if (!IsInputValid())
        {
            add_strategy_button.Focus();
            return;
        }
        var dataContext = (StrategyViewModel)DataContext;
        dataContext.AddStrategyCommand.Execute(null);
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (TextBox)sender;

        switch (e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "name_field"
                    && !string.IsNullOrEmpty(name_field.Text)) Keyboard.Focus(intermediary_field);
                if (senderRef.Name == "intermediary_field"
                    && !string.IsNullOrEmpty(intermediary_field.Text)) Keyboard.Focus(max_trade_risk_field);
                if (senderRef.Name == "max_trade_risk_field"
                    && !string.IsNullOrEmpty(max_trade_risk_field.Text)) Keyboard.Focus(daily_goal_field);
                if (senderRef.Name == "daily_goal_field"
                    && !string.IsNullOrEmpty(daily_goal_field.Text)) Keyboard.Focus(max_daily_loss_field);
                if (senderRef.Name == "max_daily_loss_field"
                    && !string.IsNullOrEmpty(max_daily_loss_field.Text)) Keyboard.Focus(goal_field);
                break;
        }
    }

    private void OnNameFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(name_field.Text))
            name_field.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void InfoIconMouseEnterHandler(object sender, MouseEventArgs e)
    {
        var senderRef = (Image)sender;

        switch(senderRef.Name)
        {
            case "max_trade_risk_info":
                max_trade_risk_description.Visibility = Visibility.Visible;
                break;
            case "daily_goal_info":
                daily_goal_description.Visibility = Visibility.Visible;
                break;
            case "max_daily_loss_info":
                max_daily_loss_description.Visibility = Visibility.Visible;
                break;
        }
    }

    private void DescriptionPopupMouseLeaveHandler(object sender, MouseEventArgs e)
    {
        var senderRef = (Border)sender;

        switch(senderRef.Name)
        {
            case "max_trade_risk_description":
                max_trade_risk_description.Visibility = Visibility.Collapsed;
                break;
            case "daily_goal_description":
                daily_goal_description.Visibility = Visibility.Collapsed;
                break;
            case "max_daily_loss_description":
                max_daily_loss_description.Visibility = Visibility.Collapsed;
                break;
        }
    }
}
