using API;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainModule.DataAccess;
using MainModule.DataModel;
using Prism.Events;
using System.Collections.ObjectModel;

namespace MainModule.ViewModels;

public partial class TradeImageViewModel : ObservableObject, IViewModel
{
    private readonly TradeImageAccess _tradeImageAccess;

    private readonly HomeViewModel _homeViewModel;

    public HomeViewModel HomeViewModel => _homeViewModel;

    private readonly INavigationHelper _mainNavigationHelper;

    public INavigationHelper MainNavigationHelper => _mainNavigationHelper;

    private readonly IEventAggregator _eventAggregator;

    public ObservableCollection<TradeImage> Images { get; } = [];

    [RelayCommand]
    private void LoadTradeImages(object trade)
    {
        Images.Clear();

        var castedTrade = (Trade)trade;

        var tempDataReckords = _tradeImageAccess.QueryTradeImagesAsync(castedTrade.Id).Result;

        foreach (var i in tempDataReckords) Images.Add(i);
    }

    [RelayCommand]
    private void AddTradeImage(byte[] image)
    {
        TradeImage newImage = new()
        {
            TradeId = _homeViewModel.SelectedTrade.Id,
            Image = image
        };

        _tradeImageAccess.InsertTradeImage(newImage);
        Images.Add(newImage);
    }

    public TradeImageViewModel(HomeViewModel homeViewModel,
                               TradeImageAccess tradeImageAccess,
                               INavigationHelper mainNavigationHelper,
                               IEventAggregator eventAggregator)
    {
        _homeViewModel = homeViewModel;
        _mainNavigationHelper = mainNavigationHelper;
        _eventAggregator = eventAggregator;
        _tradeImageAccess = tradeImageAccess;
    }
}
