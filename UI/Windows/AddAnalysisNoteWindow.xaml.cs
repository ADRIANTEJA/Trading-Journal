using MainModule.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Common.Helpers;
using UI.Controls.TextBoxes;

namespace UI.Windows;
/// <summary>
/// Interaction logic for AddAnalysisNoteWindow.xaml
/// </summary>
public partial class AddAnalysisNoteWindow : Window
{
    public AddAnalysisNoteWindow()
    {
        InitializeComponent();
    }

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private bool IsInputValid()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(title_field.Text))
        {
            title_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }

        if (string.IsNullOrEmpty(text_field.Text))
        {
            text_field.Tag = ResourceAccessHelper.ErrorRedBrush;
            isValid = false;
        }
        return isValid;
    }

    private void OnTitleFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(title_field.Text))
            title_field.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void OnTextFieldChangedHandler(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(text_field.Text))
            text_field.SetResourceReference(TagProperty, ResourceAccessHelper.ThemePlaceHolderBrushKey);
    }

    private void JumpToNextFieldHandler(object sender, KeyEventArgs e)
    {
        var senderRef = (PlaceHolderTextBox)sender;

        switch (e.Key)
        {
            case Key.Enter:
                if (senderRef.Name == "title_field"
                    && !string.IsNullOrEmpty(title_field.Text)) Keyboard.Focus(text_field);
                break;
        }
    }

    private void AddAnalysisNoteClickHandler(object sender, RoutedEventArgs e)
    {
        if (!IsInputValid())
        {
            add_note_button.Focus();
            return;
        }
        var dataContext = (AnalysisNoteViewModel)DataContext;
        dataContext.AddAnalysisNoteCommand.Execute(null);
        Close();
    }
}
