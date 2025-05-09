using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace DigitalWellBeingApp
{
    public class AppUsageTracker
    {
        private readonly Dictionary<string, TimeSpan> _usageData = new();
        private readonly Dictionary<string, int> _appSessionCounts = new();
        private string _lastApp = "";
        private DateTime _lastTime = DateTime.Now;
        private readonly DispatcherTimer _timer;

        private TimeSpan _totalUsageTime = TimeSpan.Zero;
        private int _totalSessions = 0;
        private DateTime _firstUsageDate = DateTime.Now;

        public AppUsageTracker()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += TrackAppUsage;
        }

        public void Start()
        {
            _lastTime = DateTime.Now;
            _lastApp = GetActiveWindowProcessName();

            if (!string.IsNullOrWhiteSpace(_lastApp) && !_usageData.ContainsKey(_lastApp))
                _usageData[_lastApp] = TimeSpan.Zero;

            TrackAppUsage(null, null);
            _timer.Start();
        }

        public void Stop() => _timer.Stop();

        public Dictionary<string, TimeSpan> GetUsageData() => new(_usageData);

        public Dictionary<string, int> GetAppSessionCounts() => new(_appSessionCounts);

        public TimeSpan GetTotalUsageTime() => _totalUsageTime;
        public int GetTotalSessions() => _totalSessions;
        public DateTime GetFirstUsageDate() => _firstUsageDate;

        private void TrackAppUsage(object sender, EventArgs e)
        {
            string currentApp = GetActiveWindowProcessName();
    
            if (string.IsNullOrWhiteSpace(currentApp) || currentApp == "DigitalWellBeingApp") return;

            var now = DateTime.Now;
            var elapsed = now - _lastTime;

            if (elapsed.TotalSeconds > 2)
                elapsed = TimeSpan.FromSeconds(1); 

            if (_usageData.ContainsKey(currentApp))
                _usageData[currentApp] += elapsed;
            else
                _usageData[currentApp] = elapsed;

            _totalUsageTime += elapsed;  

            if (_lastApp != currentApp)
            {
                _totalSessions++;

                if (_appSessionCounts.ContainsKey(currentApp))
                    _appSessionCounts[currentApp]++;
                else
                    _appSessionCounts[currentApp] = 1;
            }

            _lastApp = currentApp;
            _lastTime = now;

            if (_firstUsageDate == default)
                _firstUsageDate = now;
        }



        private static string GetActiveWindowProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return null;

            GetWindowThreadProcessId(hwnd, out uint pid);
            try
            {
                Process p = Process.GetProcessById((int)pid);
                return p.ProcessName;
            }
            catch
            {
                return null;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    }
}
