using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class AnalysisNoteViewModel : ObservableObject, IViewModel
{
    private readonly HomeViewModel _homeViewModel;

    private readonly NoteAccess _noteAccess;

    [ObservableProperty]
    private int strategyId;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string text;

    public ObservableCollection<AnalysisNote> Notes { get; } = [];

    public AnalysisNoteViewModel(HomeViewModel homeViewModel,
                                 NoteAccess noteAccess)
    {
        _homeViewModel = homeViewModel;
        _noteAccess = noteAccess;

    }
}
