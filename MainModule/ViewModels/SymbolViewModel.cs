using API;
using API.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace MainModule.ViewModels;

public partial class SymbolViewModel : ObservableObject, IViewModel
{
    private readonly SymbolAccess _symbolAccess;

    private readonly IEventAggregator _eventAggregator;

    private readonly INavigationHelper _navigationHelper;

    public INavigationHelper NavigationHelper => _navigationHelper;

    [ObservableProperty]
    private string soldAssetVM;

    [ObservableProperty]
    private string buyedAssetVM;

    [ObservableProperty]
    private string assetTypeVM;

    [ObservableProperty]
    private Symbol selectedSymbolVM;

    public ObservableCollection<Symbol> Symbols { get; } = [];

    [RelayCommand]
    private void LoadSymbols()
    {
        Symbols.Clear();

        var tempDataReckords = _symbolAccess.QuerySymbolsAsync().Result;

        foreach (var symbol in tempDataReckords) Symbols.Add(symbol);
    }

    [RelayCommand]
    private void LoadSymbolsByAssetType(string assetType)
    {
        Symbols.Clear();

        var tempDataReckords = _symbolAccess.QuerySymbolsByAssetTypeAsync(assetType).Result;

        foreach (var symbol in tempDataReckords) Symbols.Add(symbol);
    }

    [RelayCommand]
    private void AddSymbol()
    {
        var newSymbol = new Symbol
        {
            Pair = SoldAssetVM + "/" + BuyedAssetVM,
            AssetType = AssetTypeVM
        };

        try
        {
            _symbolAccess.InsertSymbol(newSymbol);
            Symbols.Add(newSymbol);
            _eventAggregator.GetEvent<CreateSymbolEvent>().Publish(true);
        }
        catch (SQLiteException) { _eventAggregator.GetEvent<CreateSymbolEvent>().Publish(false); }
    }

    public SymbolViewModel(SymbolAccess dataAccess, 
                           INavigationHelper navigationHelper,
                           IEventAggregator eventAggregator)
    {
        _symbolAccess = dataAccess;
        _navigationHelper = navigationHelper;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<DeleteSymbolClickEvent>().Subscribe(DeleteSymbolClickHandler);
    }

    private void DeleteSymbolClickHandler(int id)
    {
        _symbolAccess.DeleteSymbol(id);
        LoadSymbols();
    }
}
