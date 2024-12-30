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
/// Interaction logic for EditStrategyWindow.xaml
/// </summary>
public partial class EditStrategyWindow : Window
{
    public EditStrategyWindow()
    {
        InitializeComponent();
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

    private void EditStrategyClickHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)DataContext;

        var divisorFieldRef = (TextBox)risk_reward_ratio_control.FindName("divisor_field");

        var riskRewardRatioValue = divisorFieldRef.Text;

        if (!IsInputValid())
        {
            add_strategy_button.Focus();
            return;
        }

        string formerStrategyName = dataContext.SelectedStrategy.Name;

        var updatedStrategy = dataContext.SelectedStrategy;
        updatedStrategy.Name = name_field.Text;

        if (string.IsNullOrEmpty(intermediary_field.Text)) 
            updatedStrategy.Name = "";
        if (string.IsNullOrEmpty(goal_field.Text))
            updatedStrategy.Goal = "";

        updatedStrategy.RiskRewardRatio = riskRewardRatioValue;
        updatedStrategy.MaxTradeRisk = double.Parse(max_trade_risk_field.Text);
        updatedStrategy.DailyGoal = double.Parse(daily_goal_field.Text);
        updatedStrategy.MaxDailyLoss = double.Parse(max_daily_loss_field.Text);

        dataContext.UpdateStrategyCommand.Execute(updatedStrategy, formerStrategyName);

        Close();
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

    private void RiskRewardRatioControlLoadedHandler(object sender, RoutedEventArgs e)
    {
        var dataContext = (StrategyViewModel)DataContext;

        var divisorFieldRef = (TextBox)risk_reward_ratio_control.FindName("divisor_field");
        
        var binding = new Binding("RiskRewardRatio")
        {
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        divisorFieldRef.SetBinding(TextBox.TextProperty, binding);
    }
}
