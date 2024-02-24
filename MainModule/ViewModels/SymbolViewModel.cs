

using CommunityToolkit.Mvvm.ComponentModel;

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
