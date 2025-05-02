using SkiaSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.Measure;
using System.Collections.ObjectModel;

namespace DigitalWellBeingApp.ViewModels
{
    public class SleepViewModel
    {
        public ISeries[] Series { get; set; } =
        [
            new ColumnSeries<double>
            {
                Values = new ObservableCollection<double> { 9, 7.5, 7, 8, 6, 8.5, 8 },
                Name = "Sleep Duration",
                MaxBarWidth = 50,
            }
        ];

        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Name = "Time of Day",
                NamePaint = new SolidColorPaint(SKColors.White),
                Labeler = (value) => 
                {
                    return value switch
                    {
                        0 => "Mon",
                        1 => "Tue",
                        2 => "Wed",
                        3 => "Thu",
                        4 => "Fri",
                        5 => "Sat",
                        6 => "Sun",
                        _ => value.ToString()
                    };
                },
                IsVisible = true,
                MinStep = 1,
            }
        };

        public Axis[] YAxes { get; set; } =
        {
            new Axis
            {
                Name = "Sleep Duration",
                NamePaint = new SolidColorPaint(SKColors.White),
                Labeler = (value) => $"{value} hrs", 
                IsVisible = true,
                
            }
        };
    }
}
