using API;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class TradeImageViewModel : ObservableObject, IViewModel
{
    private readonly TradeImageAccess _tradeImageAccess;

    private readonly HomeViewModel _homeViewModel;

    public HomeViewModel HomeViewModel => _homeViewModel;

    private readonly INavigationHelper _mainNavigationHelper;

    public INavigationHelper MainNavigationHelper => _mainNavigationHelper;

    public ObservableCollection<TradeImage> Images { get; } = [];

    [RelayCommand]
    private async Task LoadTradeImages(object trade)
    {
        Images.Clear();

        var castedTrade = (Trade)trade;

        var tempDataReckords = await _tradeImageAccess.QueryTradeImagesAsync(castedTrade.Id);

        foreach (var image in tempDataReckords) Images.Add(image);
    }

    [RelayCommand]
    private void AddTradeImage(TradeImage tradeImageTemplate)
    {
        TradeImage newImage = new()
        {
            Id = tradeImageTemplate.Id,
            TradeId = _homeViewModel.SelectedTrade.Id,
            Image = tradeImageTemplate.Image
        };

        _tradeImageAccess.InsertTradeImage(newImage);
        Images.Add(newImage);
    }

    [RelayCommand]
    private void DeleteTradeImage(int id) 
    {
        _tradeImageAccess.DeleteTradeImage(id);

        foreach (var image in Images.Where(x => x.Id == id).ToList()) Images.Remove(image);
    }

    public TradeImageViewModel(HomeViewModel homeViewModel,
                               TradeImageAccess tradeImageAccess,
                               INavigationHelper mainNavigationHelper)
    {
        _homeViewModel = homeViewModel;
        _mainNavigationHelper = mainNavigationHelper;
        _tradeImageAccess = tradeImageAccess;
    }
}
