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
        private string _lastApp = "";
        private DateTime _lastTime = DateTime.Now;
        private readonly DispatcherTimer _timer;

        private TimeSpan _totalUsageTime = TimeSpan.Zero;
        private int _totalSessions = 0;
        private DateTime _firstUsageDate = DateTime.Now;

        public AppUsageTracker()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); 
            _timer.Tick += TrackAppUsage;
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public Dictionary<string, TimeSpan> GetUsageData()
        {
            return new Dictionary<string, TimeSpan>(_usageData); 
        }

        public TimeSpan GetTotalUsageTime() => _totalUsageTime;
        public int GetTotalSessions() => _totalSessions;
        public DateTime GetFirstUsageDate() => _firstUsageDate;

        private void TrackAppUsage(object sender, EventArgs e)
        {
            string currentApp = GetActiveWindowProcessName();

            if (string.IsNullOrWhiteSpace(currentApp)) return;

            var now = DateTime.Now;
            var elapsed = now - _lastTime;

            if (!string.IsNullOrWhiteSpace(_lastApp))
            {
                if (_usageData.ContainsKey(_lastApp))
                    _usageData[_lastApp] += elapsed;
                else
                    _usageData[_lastApp] = elapsed;
            }

            _totalUsageTime += elapsed;

            if (_lastApp != currentApp)
            {
                _totalSessions++;
                _lastApp = currentApp;
                _firstUsageDate = _firstUsageDate == DateTime.Now ? now : _firstUsageDate;
            }

            _lastTime = now;
        }

        private static string GetActiveWindowProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return null;

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
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
