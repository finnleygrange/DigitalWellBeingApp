using DigitalWellBeingApp.Data;
using DigitalWellBeingApp.Models;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SleepViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<double> _sleepValues = new();
    private string[] _xLabels = Array.Empty<string>();

    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }

    public SleepViewModel()
    {
        LoadData();
    }

    public void LoadData()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        var sleepData = db.SleepEntries
            .OrderBy(e => e.Date)
            .ToList();

        _sleepValues = new ObservableCollection<double>(sleepData.Select(e => e.HoursSlept));
        _xLabels = sleepData.Select(e => e.Date.ToString("ddd")).ToArray();

        Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = _sleepValues,
                Name = "Sleep Duration",
                MaxBarWidth = 50,
                Fill = new SolidColorPaint(SKColors.SkyBlue)
            }
        };

        XAxes = new Axis[]
        {
            new Axis
            {
                Name = "Day",
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

        OnPropertyChanged(nameof(Series));
        OnPropertyChanged(nameof(XAxes));
        OnPropertyChanged(nameof(YAxes));
    }

    public void AddSleepEntry(DateTime date, double hoursSlept)
    {
        using var db = new AppDbContext();
        db.SleepEntries.Add(new SleepEntry
        {
            Date = date,
            HoursSlept = hoursSlept
        });
        db.SaveChanges();

        LoadData(); 
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
