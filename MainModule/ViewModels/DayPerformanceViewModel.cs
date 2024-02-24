using CommunityToolkit.Mvvm.ComponentModel;

namespace MainModule.ViewModels;

public partial class DayPerformanceViewModel : ObservableObject, IViewModel
{
    [ObservableProperty]
    private int accountId;

    [ObservableProperty]
    private DateTime date;

    [ObservableProperty]
    private double _ROI;

    [ObservableProperty]
    private double _ROIPercentage;
}
