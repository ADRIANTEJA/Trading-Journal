using API;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class AnalysisNoteViewModel : ObservableObject, IViewModel
{
    private readonly INavigationHelper _navigationHelper;

    public INavigationHelper NavigationHelper => _navigationHelper;

    private readonly HomeViewModel _homeViewModel;

    private readonly AnalysisNoteAccess _noteAccess;

    private readonly StrategyViewModel _strategyViewModel;

    public StrategyViewModel StrategyViewModel => _strategyViewModel;

    [ObservableProperty]
    private string titleVM;

    [ObservableProperty]
    private string textVM;

    public ObservableCollection<AnalysisNote> AnalysisNotes { get; } = [];

    [RelayCommand]
    private void LoadAnalysisNotes(object strategy)
    {
        AnalysisNotes.Clear();

        var castedStrategy = (Strategy)strategy;

        var tempDataReckords = _noteAccess.QueryStrategyAnalysisNotesAsync(castedStrategy.Id).Result;

        foreach (var note in tempDataReckords) AnalysisNotes.Add(note);
    }

    [RelayCommand]
    private void AddAnalysisNote()
    {
        var newNote = new AnalysisNote
        {
            StrategyId = _strategyViewModel.SelectedStrategy.Id,
            Title = TitleVM,
            Text = TextVM,
        };

        _noteAccess.InsertTradeNote(newNote);
        AnalysisNotes.Add(newNote);
    }

    public AnalysisNoteViewModel(INavigationHelper navigationHelper,
                                 StrategyViewModel strategyViewModel,
                                 AnalysisNoteAccess noteAccess)
    {
        _navigationHelper = navigationHelper;
        _strategyViewModel = strategyViewModel;
        _noteAccess = noteAccess;
    }
}
