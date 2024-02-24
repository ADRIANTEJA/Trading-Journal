using CommunityToolkit.Mvvm.ComponentModel;

namespace MainModule.ViewModels;

public partial class AnalysisNoteViewModel : ObservableObject, IViewModel
{
    [ObservableProperty]
    private int strategyId;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string text;
}
