using CommunityToolkit.Mvvm.ComponentModel;

namespace MainModule.ViewModels;

public partial class TradeImageViewModel : ObservableObject, IViewModel
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private byte[] image;
}
