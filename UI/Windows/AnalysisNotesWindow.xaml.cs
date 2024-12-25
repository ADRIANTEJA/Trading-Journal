using API;
using MainModule.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace UI.Windows;
/// <summary>
/// Interaction logic for AnalysisNotesWindow.xaml
/// </summary>
public partial class AnalysisNotesWindow : Window
{
    public AnalysisNotesWindow()
    {
        InitializeComponent();
    }

    private void MinimizeWindowHandler(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseWindowHandler(object sender, RoutedEventArgs e) => Close();

    private void DragMoveHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    private void OnAnlylisisNotesWindowLoaded(object sender, RoutedEventArgs e)
    {
        var dataContext = (AnalysisNoteViewModel)DataContext;
        dataContext.LoadAnalysisNotesCommand.Execute(dataContext.StrategyViewModel.SelectedStrategy.Id);
    }

}
