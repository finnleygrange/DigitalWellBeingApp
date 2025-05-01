using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
    /// <summary>
    /// Interaction logic for Pomodoro.xaml
    /// </summary>
    public partial class Pomodoro : UserControl
    {
        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;
        private bool _isRunning = false;
        private int _pomodoroCount = 0;

        private bool IsTesting = true;

        public Pomodoro()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            SetTimer(TimeSpan.FromMinutes(25)); 
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0)
            {
                _timeLeft = _timeLeft - TimeSpan.FromSeconds(1);
                UpdateTimerDisplay();
            }
            else
            {
                _timer.Stop();
                btnStart.Content = "START";
                _isRunning = false;

                if (btnPomodoro.IsChecked == true)
                    _pomodoroCount++;

                PlayCustomAlarm();
            }
        }

        private void SetTimer(TimeSpan duration)
        {
            _timeLeft = duration;
            UpdateTimerDisplay();
        }

        private void PlayCustomAlarm()
        {
            string soundPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(),"Resources", "Sounds", "alarm.wav");

            if (File.Exists(soundPath))
            {
                try
                {
                    SoundPlayer player = new SoundPlayer(soundPath);
                    player.PlayLooping(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error playing sound: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Alarm sound file not found.");
            }
        }

        private void UpdateTimerDisplay()
        {
            if (TimerText != null)
            {
                TimerText.Text = _timeLeft.ToString(@"mm\:ss");
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                _timer.Stop();
                btnStart.Content = "START";
            }
            else
            {
                _timer.Start();
                btnStart.Content = "PAUSE";
            }

            _isRunning = !_isRunning;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            _isRunning = false;
            _timer.Stop();
            btnStart.Content = "START";

            if (btnPomodoro.IsChecked == true)
            {
                SetTimer(TimeSpan.FromMinutes(25));
            }
            else if (btnShortBreak.IsChecked == true)
            {
                SetTimer(TimeSpan.FromMinutes(5));
            }
            else if (btnLongBreak.IsChecked == true)
            {
                SetTimer(TimeSpan.FromMinutes(15));
            }
        }

        private void btnPomodoro_Checked(object sender, RoutedEventArgs e)
        {
            SetTimer(GetDurationForMode("Pomodoro"));
        }

        private void btnShortBreak_Checked(object sender, RoutedEventArgs e)
        {
            SetTimer(GetDurationForMode("ShortBreak"));
        }

        private void btnLongBreak_Checked(object sender, RoutedEventArgs e)
        {
            SetTimer(GetDurationForMode("LongBreak"));
        }

        private TimeSpan GetDurationForMode(string mode)
        {
            switch (mode)
            {
                case "Pomodoro":
                    return IsTesting ? TimeSpan.FromSeconds(10) : TimeSpan.FromMinutes(25);
                case "ShortBreak":
                    return IsTesting ? TimeSpan.FromSeconds(5) : TimeSpan.FromMinutes(5);
                case "LongBreak":
                    return IsTesting ? TimeSpan.FromSeconds(8) : TimeSpan.FromMinutes(15);
                default:
                    return TimeSpan.FromMinutes(25);
            }
        }
    }

}
