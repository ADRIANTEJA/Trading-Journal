using API.Events;
using MainModule.ViewModels;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UI.Common.Helpers;

namespace UI.Windows;
/// <summary>
/// Interaction logic for AddSymbolWindow.xaml
/// </summary>
public partial class AddSymbolWindow : Window
{
    public AddSymbolWindow(IEventAggregator eventAggregator)
    {
        InitializeComponent();
        eventAggregator.GetEvent<OnCreateSymbolEvent>().Subscribe(SymbolCreationHandler);
    }

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

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

    private void SymbolCreationHandler(bool success)
    {
        if (success) Close();
        else
        {
            input_error_textblock.Text = 
            Application.Current.FindResource(ResourceAccessHelper.DuplicatedSymbolNameErrorKey).ToString();
            input_error_textblock.Visibility = Visibility.Visible;
        }
    }

    private void OnAssetTypeSelectorLoaded(object sender, RoutedEventArgs e)
    {
        var optionListRef = (ListView)asset_type_selector.FindName("options_listview");
        optionListRef.SetBinding(ListView.SelectedValueProperty, "AssetTypeVM");
    }

    private bool IsInputValid()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(sold_asset_field.Text))
        {
            sold_asset_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(buyed_asset_field.Text))
        {
            buyed_asset_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        var assetTypeListRef = (ListView)asset_type_selector.FindName("options_listview");

        if (assetTypeListRef.SelectedValue == null)
        {
            input_error_textblock.Text =
            Application.Current.FindResource(ResourceAccessHelper.AddSymbolWindowAssetTypeErrorMessageKey).ToString();
            input_error_textblock.Visibility = Visibility.Visible;
        }

        return isValid;
    }

    private void AddSymbolClickHandler(object sender, RoutedEventArgs e)
    {
        if (!IsInputValid())
        {
            add_symbol_button.Focus();
            return;
        }
        var dataContext = (SymbolViewModel)DataContext;
        dataContext.AddSymbolCommand.Execute(null);
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (TextBox)sender;

        switch (e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "sold_asset_field"
                    && !string.IsNullOrEmpty(sold_asset_field.Text)) Keyboard.Focus(buyed_asset_field);
                break;
        }
    }

    private void OnAssetPairChangedHandler(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(sold_asset_field.Text))
            sold_asset_field.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);

        if (!string.IsNullOrEmpty(buyed_asset_field.Text))
            buyed_asset_field.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }
}
