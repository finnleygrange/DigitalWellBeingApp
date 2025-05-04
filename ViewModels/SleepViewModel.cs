using DigitalWellBeingApp.Data;
using DigitalWellBeingApp.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

public enum SleepFilter
{
    Week,
    Month,
    SixMonths
}

public class SleepViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<double> _sleepValues = new();
    private string[] _xLabels = Array.Empty<string>();

    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }

    private SleepFilter _currentFilter = SleepFilter.Week;

    public SleepViewModel()
    {
        LoadData();
    }

    public void SetFilter(SleepFilter filter)
    {
        _currentFilter = filter;
        LoadData();
    }

    public void LoadData()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        var now = DateTime.Now;
        var entries = db.SleepEntries.ToList();

        Dictionary<string, double> dataMap = new();
        List<string> labels = new();
        double totalSleep = 0;
        double numberOfEntries = 0;


        switch (_currentFilter)
        {
            case SleepFilter.Week:
                DateTime startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + (now.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
                for (int i = 0; i < 7; i++)
                {
                    var day = startOfWeek.AddDays(i);
                    string label = day.ToString("ddd"); 
                    labels.Add(label);

                    var totalForDay = entries
                        .Where(e => e.Date.Date == day)
                        .Sum(e => e.HoursSlept);

                    dataMap[label] = totalForDay > 0 ? totalForDay : 0;

                    totalSleep += totalForDay;
                    if (totalForDay > 0) numberOfEntries++;
                }
                break;

            case SleepFilter.Month:
                DateTime firstOfMonth = new DateTime(now.Year, now.Month, 1);
                int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

                for (int i = 0; i < daysInMonth; i++)
                {
                    var day = firstOfMonth.AddDays(i);
                    string label = day.ToString("dd");
                    labels.Add(label);

                    var totalForDay = entries.Where(e => e.Date.Date == day).Sum(e => e.HoursSlept);

                    dataMap[label] = totalForDay > 0 ? totalForDay : 0;

                    totalSleep += totalForDay;
                    if (totalForDay > 0) numberOfEntries++; 
                }
                break;

            case SleepFilter.SixMonths:
                for (int i = 5; i >= 0; i--)
                {
                    var month = now.AddMonths(-i);
                    string label = month.ToString("MMM"); 
                    labels.Add(label);

                    var entriesForMonth = entries
                        .Where(e => e.Date.Year == month.Year && e.Date.Month == month.Month)
                        .ToList();

                    if (entriesForMonth.Any())
                    {
                        var totalForMonth = entriesForMonth.Sum(e => e.HoursSlept);

                        var daysWithData = entriesForMonth
                            .GroupBy(e => e.Date.Date)  
                            .Count();

                        var avgSleepForMonth = totalForMonth / daysWithData;

                        dataMap[label] = avgSleepForMonth;
                    }
                    else
                    {
                        dataMap[label] = 0;
                    }

                    totalSleep += dataMap[label];
                    if (dataMap[label] > 0) numberOfEntries++; 
                }
                break;

        }

        _xLabels = labels.ToArray();
        _sleepValues = new ObservableCollection<double>(_xLabels.Select(l =>
            dataMap.ContainsKey(l) ? dataMap[l] : 0)); 

        Series = new ISeries[]
        {
        new ColumnSeries<double>
        {
            Values = _sleepValues,
            Name = _currentFilter == SleepFilter.SixMonths ? "Avg Sleep per Month" : "Sleep Duration",
            MaxBarWidth = 50,
            Fill = new SolidColorPaint(SKColors.SkyBlue)
        }
        };

        XAxes = new Axis[]
        {
        new Axis
        {
            Name = "Date",
            Labels = _xLabels,
            NamePaint = new SolidColorPaint(SKColors.White),
            TextSize = 14,
            MinStep = 1
        }
        };

        YAxes = new Axis[]
        {
        new Axis
        {
            Name = "Hours",
            Labeler = value => $"{value} hrs",
            NamePaint = new SolidColorPaint(SKColors.White),
            MinLimit = 0
        }
        };

        double averageSleep = numberOfEntries > 0 ? totalSleep / numberOfEntries : 0;

        AverageSleep = averageSleep;

        OnPropertyChanged(nameof(Series));
        OnPropertyChanged(nameof(XAxes));
        OnPropertyChanged(nameof(YAxes));
    }



    private double _averageSleep;
    public double AverageSleep
    {
        get => _averageSleep;
        set
        {
            _averageSleep = value;
            OnPropertyChanged();
        }
    }

    public void AddSleepEntry(DateTime date, double hoursSlept)
    {
        using var db = new AppDbContext();
        var entry = new SleepEntry
        {
            Date = date,
            HoursSlept = hoursSlept
        };
        db.SleepEntries.Add(entry);
        db.SaveChanges();

        LoadData();
    }


    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

