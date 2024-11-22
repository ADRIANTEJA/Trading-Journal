using API.Events;
using MainModule.ViewModels;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
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

        if (string.IsNullOrEmpty(risk_reward_ratio_field.Text))
        {
            risk_reward_ratio_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(max_trade_risk_field.Text))
        {
            max_trade_risk_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(daily_goal_field.Text))
        {
            daily_goal_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(max_daily_loss_field.Text))
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
                    && !string.IsNullOrEmpty(intermediary_field.Text)) Keyboard.Focus(risk_reward_ratio_field);
                if (senderRef.Name == "risk_reward_ratio_field"
                    && !string.IsNullOrEmpty(risk_reward_ratio_field.Text)) Keyboard.Focus(max_trade_risk_field);
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

    private void OnAssetTypeSelectionChangedHandler(object sender, RoutedEventArgs e)
    {
        var optionsListRef = (ListView)asset_type_selector.FindName("options_listview");
        optionsListRef.SelectionChanged += AssetTypeChangedHandler;
    }

    private void AssetTypeChangedHandler(object sender, SelectionChangedEventArgs e)
    {
        var optionsListRef = (ListView)asset_type_selector.FindName("options_listview");

        var dataContext = (StrategyViewModel)DataContext;
        dataContext.SymbolViewModel.LoadSymbolsByAssetTypeCommand.Execute(optionsListRef.SelectedValue.ToString());
    }
}
