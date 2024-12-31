using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts.Defaults;
using LiveCharts;
using MainModule.DataAccess;
using MainModule.DataModel;
using static MainModule.Common.Enums;
using CommunityToolkit.Mvvm.Input;

namespace MainModule.ViewModels;

public partial class PerformanceViewModel : ObservableObject, IViewModel
{
    private PerformanceAccess _performanceAccess;

    [ObservableProperty]
    private ROIFormat roiFormat = ROIFormat.Value;

    [ObservableProperty]
    private PerfomanceTimeFrame accountPerformanceTimeFrame = PerfomanceTimeFrame.Daily;

    public ChartValues<ObservablePoint> AccountPerformance { get; } = [];

    [RelayCommand]
    public async Task LoadDailyPerformance(int accountId)
    {
        AccountPerformance.Clear();

        var tempReckordsList = await _performanceAccess.QueryPerformanceByAccountIdAsync(accountId);

        List<Performance> performance = [];

        switch (AccountPerformanceTimeFrame)
        {
            case PerfomanceTimeFrame.Daily:

                performance = tempReckordsList
                    .Select(x => new { Date = new DateTime(x.Date), x.ROI, x.ROIPercentage, x.Cost })
                    .GroupBy(x => new { x.Date.Year, x.Date.Month, x.Date.Day })
                    .Select(g => new Performance
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day).Ticks,
                        ROI = g.Sum(x => x.ROI),
                        Cost = g.Sum(x => x.Cost),
                        ROIPercentage = g.Sum(x => x.ROI) / g.Sum(x => x.Cost) * 100
                    }).ToList(); 
                break;
            case PerfomanceTimeFrame.Monthly:

                performance = tempReckordsList
                    .Select(x => new { Date = new DateTime(x.Date), x.ROI, x.ROIPercentage, x.Cost })
                    .GroupBy(x => new { x.Date.Month, x.Date.Year })
                    .Select(g => new Performance
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1).Ticks,
                        ROI = g.Sum(x => x.ROI),
                        Cost = g.Sum(x => x.Cost),
                        ROIPercentage = g.Sum(x => x.ROI) / g.Sum(x => x.Cost) * 100
                    }).ToList();
                break;
            case PerfomanceTimeFrame.Yearly:

                performance = tempReckordsList
                    .Select(x => new { Date = new DateTime(x.Date), x.ROI, x.ROIPercentage, x.Cost })
                    .GroupBy(x => new { x.Date.Year })
                    .Select(g => new Performance
                    {
                        Date = new DateTime(g.Key.Year, 1, 1).Ticks,
                        ROI = g.Sum(x => x.ROI),
                        Cost = g.Sum(x => x.Cost),
                        ROIPercentage = g.Sum(x => x.ROI) / g.Sum(x => x.Cost) * 100
                    }).ToList();
                break;
        }

        foreach (var i in performance) AccountPerformance.Add(new(i.Date, i.ROIPercentage));
    }
    [RelayCommand]
    private void FilterAccountPerformanceByDate(long dateTicks)
    {
        ObservablePoint performancePoint = null!;

        performancePoint = (from point in AccountPerformance
                            where point.X == dateTicks
                            select new ObservablePoint
                            {
                                X = point.X,
                                Y = point.Y,
                            }).ToList()[0];

        AccountPerformance.Clear();
        AccountPerformance.Add(performancePoint);
    }

    public PerformanceViewModel(PerformanceAccess performanceAccess)
    {
        _performanceAccess = performanceAccess;
    }

    public void DeletePerformanceByDate(long dateTicks)
    {
        _performanceAccess.DeletePerformanceByDate(dateTicks);
    }

    public void AddAccountPerformanceRecord(Performance newPerformanceRecord)
    {
        _performanceAccess.InsertPerformance(newPerformanceRecord);
    }
}
