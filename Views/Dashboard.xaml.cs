using DigitalWellBeingApp.Data;
using DigitalWellBeingApp.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DigitalWellBeingApp.Views
{
    public partial class Dashboard : UserControl
    {
        private DispatcherTimer _updateTimer;

        public Dashboard()
        {
            InitializeComponent();

            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += UpdateDashboardData;
            _updateTimer.Start();

            UpdateDashboardData(null, null);
        }

        private void UpdateDashboardData(object sender, EventArgs e)
        {
            var appUsageData = LoadAppUsageData();
            var sleepData = LoadSleepData();
            var pomodoroData = LoadPomodoroData();

            TotalUsageText.Text = $"Total App Usage: {FormatTimeSpan(appUsageData.TotalUsage)}";
            TotalSessionsText.Text = $"Total Sessions: {appUsageData.TotalSessions}";
            TotalSleepText.Text = $"Total Sleep: {FormatTimeSpan(sleepData.TotalSleep)}";
            SleepSessionsText.Text = $"Total Sleep Sessions: {sleepData.SleepSessions}";
            PomodoroCompletionText.Text = $"Pomodoros Completed: {pomodoroData.CompletedPomodoros}";
        }

        private (TimeSpan TotalUsage, int TotalSessions) LoadAppUsageData()
        {
            using (var db = new AppDbContext())
            {
                var appUsageData = db.AppUsageData.ToList();

                TimeSpan totalUsage = TimeSpan.Zero;
                int totalSessions = 0;

                foreach (var data in appUsageData)
                {
                    totalUsage += TimeSpan.FromSeconds(data.TimeSpent);
                    totalSessions += data.SessionCount;
                }

                return (totalUsage, totalSessions);
            }
        }

        private (TimeSpan TotalSleep, int SleepSessions) LoadSleepData()
        {
            using (var db = new AppDbContext())
            {
                var sleepData = db.SleepEntries.ToList(); 

                TimeSpan totalSleep = TimeSpan.Zero;
                int sleepSessions = sleepData.Count;

                foreach (var data in sleepData)
                {
                    totalSleep += TimeSpan.FromHours(data.HoursSlept); 
                }

                return (totalSleep, sleepSessions);
            }
        }

        private (int CompletedPomodoros, string Message) LoadPomodoroData()
        {
            using (var db = new AppDbContext())
            {
                var pomodoroData = db.PomodoroData
                    .FirstOrDefault(p => p.Date.Date == DateTime.Now.Date);

                if (pomodoroData != null)
                {
                    return (pomodoroData.CompletedPomodoros, "Data found for today");
                }
                else
                {
                    return (0, "No Pomodoro data found for today");
                }
            }
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
        }
    }
}
