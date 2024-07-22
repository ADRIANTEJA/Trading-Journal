

using CommunityToolkit.Mvvm.ComponentModel;
using MainModule.Common;

namespace MainModule.ViewModels;

public partial class SymbolViewModel : ObservableObject, IViewModel
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string pair;

    [ObservableProperty]
    private string assetType;
}
