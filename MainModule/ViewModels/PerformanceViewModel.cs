using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts.Defaults;
using LiveCharts;
using MainModule.DataAccess;
using MainModule.DataModel;
using System.Collections.ObjectModel;
using static MainModule.Common.Enums;
using API.Events;

namespace MainModule.ViewModels;

public partial class PerformanceViewModel : ObservableObject, IViewModel
{
    private PerformanceAccess _performanceAccess;

    private IEventAggregator _eventAggregator;

    private ROIFormat roiFormat = ROIFormat.Value;

    private PerfomanceTimeFrame performanceTimeFrame = PerfomanceTimeFrame.Daily;

    [ObservableProperty]
    private DateTime date;

    [ObservableProperty]
    private double _ROI;

    [ObservableProperty]
    private double _ROIPercentage;

    public ObservableCollection<Performance> PerformanceRecords { get; } = [];

    [ObservableProperty]
    private ChartValues<ObservablePoint> accountPerformance = [];

    public PerformanceViewModel(PerformanceAccess performanceAccess, 
                                IEventAggregator eventAggregator)
    {
        _performanceAccess = performanceAccess;
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<CreatePerformanceEvent>().Subscribe(LoadDailyPerformanceHandler);
    }

    //Fix this crap
    private void LoadDailyPerformanceHandler(int accountId)
    {
        PerformanceRecords.Clear();

        var tempReckordsList = _performanceAccess.QueryDayPerformanceByAccountIdAsync(accountId).Result;

        List<Performance> performance = [];

        switch (performanceTimeFrame)
        {
            case PerfomanceTimeFrame.Daily:

                performance = tempReckordsList;
                break;
            case PerfomanceTimeFrame.Monthly:

                performance = tempReckordsList
                    .Select(x => new { Date = new DateTime(x.Date), x.ROI, x.ROIPercentage })
                    .GroupBy(x => new { x.Date.Month, x.Date.Year })
                    .Select(g => new Performance
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1).Ticks,
                        ROI = g.Sum(x => x.ROI),
                        ROIPercentage = g.Sum(x => x.ROIPercentage) //Fix the ROI percentage calculation formula
                    }).ToList();
                break;
        }

        switch (roiFormat)
        {
            case ROIFormat.Value:

                foreach (var i in performance) AccountPerformance.Add(new(DateTime.Now.Ticks, i.ROI));
                break;
            case ROIFormat.Percentage:

                foreach (var i in performance) AccountPerformance.Add(new(DateTime.Now.Ticks, i.ROIPercentage));
                break;
        }
    }

    public void AddAccountPerformanceRecord(int accountId, long date, double roi, double roiPercentage)
    {
        var newPerformanceRecord = new Performance
        {
            AccountId = accountId,
            Date = date,
            ROI = roi,
            ROIPercentage = roiPercentage
        };

        _performanceAccess.InsertDayPerformance(newPerformanceRecord);
        PerformanceRecords.Add(newPerformanceRecord);
    }
}
