using DigitalWellBeingApp.Data;
using DigitalWellBeingApp.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DigitalWellBeingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AppUsageTracker _usageTracker;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _usageTracker = new AppUsageTracker();
            _usageTracker.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _usageTracker.Stop();
            base.OnExit(e);
        }
    }

}
