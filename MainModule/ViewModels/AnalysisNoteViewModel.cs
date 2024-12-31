using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class AnalysisNoteViewModel : ObservableObject, IViewModel
{
    private readonly IEventAggregator _eventAggregator;

    private readonly INavigationHelper _navigationHelper;

    public INavigationHelper NavigationHelper => _navigationHelper;

    private readonly AnalysisNoteAccess _noteAccess;

    private readonly StrategyViewModel _strategyViewModel;

    public StrategyViewModel StrategyViewModel => _strategyViewModel;

    [ObservableProperty]
    private string titleVM;

    [ObservableProperty]
    private string textVM;

    public ObservableCollection<AnalysisNote> AnalysisNotes { get; } = [];

    [RelayCommand]
    private async Task LoadAnalysisNotes(int id)
    {
        AnalysisNotes.Clear();

        var tempDataReckords = await _noteAccess.QueryStrategyAnalysisNotesAsync(id);

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

    public AnalysisNoteViewModel(IEventAggregator eventAggregator,
                                 INavigationHelper navigationHelper,
                                 StrategyViewModel strategyViewModel,
                                 AnalysisNoteAccess noteAccess)
    {
        _eventAggregator = eventAggregator;
        _navigationHelper = navigationHelper;
        _strategyViewModel = strategyViewModel;
        _noteAccess = noteAccess;

        _eventAggregator.GetEvent<DeleteNoteClickEvent>().Subscribe(OnDeleteNoteClickHandler);
    }

    private void OnDeleteNoteClickHandler(int id)
    {
        _noteAccess.DeleteTradeNote(id);
        _ = LoadAnalysisNotes(StrategyViewModel.SelectedStrategy.Id);
    }
}
