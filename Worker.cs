using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace DigitalWellBeingApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string _previousActiveWindow = string.Empty;
        private Stopwatch _stopwatch = new Stopwatch();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stopwatch.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                TrackActiveApplication();
                
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void TrackActiveApplication()
        {
            string activeWindow = GetActiveApplication();

            if (activeWindow != _previousActiveWindow)
            {
                if (!string.IsNullOrEmpty(_previousActiveWindow))
                {
                    _logger.LogInformation($"Spent {_stopwatch.Elapsed} seconds on {_previousActiveWindow}");
                }

                _previousActiveWindow = activeWindow;
                _stopwatch.Restart();
            }

            _logger.LogInformation($"Active Window: {activeWindow}, Time Spent: {_stopwatch.Elapsed}s");
        }


        private string GetActiveApplication()
        {
            IntPtr hWnd = GetForegroundWindow();
            GetWindowThreadProcessId(hWnd, out uint pid);

            try
            {
                using Process proc = Process.GetProcessById((int)pid);
                return proc.ProcessName;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error retrieving active application: {ex.Message}");
                return "Unknown";
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
}
