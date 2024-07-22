using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.Common;

namespace MainModule.ViewModels;

public partial class TradeImageViewModel : ObservableObject, IViewModel
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private byte[] image;
}
