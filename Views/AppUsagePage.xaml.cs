using DigitalWellBeingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            _timer.Tick += (sender, args) => PopulateAppUsageData();
            _timer.Start();
        }

        private void PopulateAppUsageData()
        {
            var appUsageData = new List<AppUsageData>();
            foreach (var app in _appUsageTracker.GetUsageData())
            {
                appUsageData.Add(new AppUsageData
                {
                    AppName = app.Key,
                    TimeSpent = (int)app.Value.TotalSeconds
                });
            }

            AppUsageListView.ItemsSource = appUsageData;

            UpdateMetrics();
        }

        private void UpdateMetrics()
        {
            var totalUsage = _appUsageTracker.GetTotalUsageTime();
            TotalUsageText.Text = $"Total Usage: {(int)totalUsage.TotalSeconds} sec";

            var daysSinceFirstUsage = (DateTime.Now - _appUsageTracker.GetFirstUsageDate()).Days;
            var averageDailyUsage = (daysSinceFirstUsage > 0)
                                    ? (int)(totalUsage.TotalSeconds / daysSinceFirstUsage) 
                                    : 0;

            AverageDailyUsageText.Text = $"Average Daily Usage: {averageDailyUsage} sec";

            var totalSessions = _appUsageTracker.GetTotalSessions();
            TotalSessionsText.Text = $"Total Sessions: {totalSessions}";
        }


    }
}
