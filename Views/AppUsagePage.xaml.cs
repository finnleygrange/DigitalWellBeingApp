using DigitalWellBeingApp.Data;
using DigitalWellBeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DigitalWellBeingApp.Views
{
    public partial class AppUsagePage : UserControl
    {
        private AppUsageTracker _appUsageTracker;
        private DispatcherTimer _timer;

        public AppUsagePage()
        {
            InitializeComponent();

            _appUsageTracker = new AppUsageTracker();
            _appUsageTracker.Start();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) => UpdateMetrics();
            _timer.Start();

            UpdateMetrics();
        }

        private void UpdateMetrics()
        {
            var totalUsage = _appUsageTracker.GetTotalUsageTime().TotalSeconds;  
            var totalSessions = _appUsageTracker.GetTotalSessions();  

            TotalUsageText.Text = $"Total Usage: {FormatTimeSpan(TimeSpan.FromSeconds(totalUsage))}";
            TotalSessionsText.Text = $"Total Sessions: {totalSessions}";

            PopulateAppUsageData();
        }



        private void PopulateAppUsageData()
        {
            var appUsageData = new List<AppUsageData>();
            var sessionCounts = _appUsageTracker.GetAppSessionCounts();

            foreach (var app in _appUsageTracker.GetUsageData())
            {
                if (app.Key == "DigitalWellBeingApp") continue;

                int sessions = sessionCounts.ContainsKey(app.Key) ? sessionCounts[app.Key] : 0;
                int totalSeconds = (int)app.Value.TotalSeconds;

                appUsageData.Add(new AppUsageData
                {
                    AppName = $"{app.Key} ({sessions} session{(sessions == 1 ? "" : "s")})",
                    TimeSpent = totalSeconds
                });
            }

            AppUsageListView.ItemsSource = appUsageData;
        }
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
        }

    }
}
