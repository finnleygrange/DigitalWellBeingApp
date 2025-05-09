using DigitalWellBeingApp.Data;
using DigitalWellBeingApp.Models;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DigitalWellBeingApp.Views
{
    public partial class Pomodoro : UserControl
    {
        private readonly DispatcherTimer _timer;
        private TimeSpan _timeLeft;
        private bool _isRunning = false;
        private int _pomodoroCount = 0;

        private readonly bool _isTesting = true;

        public Pomodoro()
        {
            InitializeComponent();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;

            SetTimer(GetDurationForMode("Pomodoro"));
            LoadPomodoroCountFromDb();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0)
            {
                _timeLeft -= TimeSpan.FromSeconds(1);
                UpdateTimerDisplay();
            }
            else
            {
                _timer.Stop();
                _isRunning = false;
                btnStart.Content = "START";

                if (btnPomodoro.IsChecked == true)
                    _pomodoroCount++;

                SavePomodoroCountToDb();

                UpdatePomodoroCountDisplay();
                PlayCustomAlarm();
            }
        }

        private void UpdatePomodoroCountDisplay()
        {
            PomodoroCountText.Text = $"Pomodoros Completed: {_pomodoroCount}";
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            string soundPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Sounds", "click.wav");

            if (!_isRunning && _timeLeft.TotalSeconds == 0)
            {
                return;
            }

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

            SoundPlayer player = new SoundPlayer(soundPath);
            player.Play();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _isRunning = false;
            btnStart.Content = "START";

            if (btnPomodoro.IsChecked == true)
                SetTimer(GetDurationForMode("Pomodoro"));
            else if (btnShortBreak.IsChecked == true)
                SetTimer(GetDurationForMode("ShortBreak"));
            else if (btnLongBreak.IsChecked == true)
                SetTimer(GetDurationForMode("LongBreak"));
        }

        private void SetTimer(TimeSpan duration)
        {
            _timeLeft = duration;
            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            if (TimerText != null)
                TimerText.Text = _timeLeft.ToString(@"mm\:ss");
        }

        private TimeSpan GetDurationForMode(string mode)
        {
            return mode switch
            {
                "Pomodoro" => _isTesting ? TimeSpan.FromSeconds(10) : TimeSpan.FromMinutes(25),
                "ShortBreak" => _isTesting ? TimeSpan.FromSeconds(5) : TimeSpan.FromMinutes(5),
                "LongBreak" => _isTesting ? TimeSpan.FromSeconds(8) : TimeSpan.FromMinutes(15),
                _ => TimeSpan.FromMinutes(25),
            };
        }

        private void SavePomodoroCountToDb()
        {
            using (var db = new AppDbContext())
            {
                var pomodoroData = db.PomodoroData
                    .FirstOrDefault(p => p.Date.Date == DateTime.Now.Date);

                if (pomodoroData == null)
                {
                    pomodoroData = new PomodoroData
                    {
                        Date = DateTime.Now.Date,
                        CompletedPomodoros = _pomodoroCount
                    };
                    db.PomodoroData.Add(pomodoroData);
                }
                else
                {
                    pomodoroData.CompletedPomodoros = _pomodoroCount;
                }

                db.SaveChanges();
            }
        }

        private void LoadPomodoroCountFromDb()
        {
            using (var db = new AppDbContext())
            {
                var pomodoroData = db.PomodoroData
                    .FirstOrDefault(p => p.Date.Date == DateTime.Now.Date);

                if (pomodoroData != null)
                {
                    _pomodoroCount = pomodoroData.CompletedPomodoros;
                    UpdatePomodoroCountDisplay();
                }
            }
        }

        private void PlayCustomAlarm()
        {
            string soundPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Sounds", "alarm.wav");

            if (!File.Exists(soundPath))
            {
                MessageBox.Show("Alarm sound file not found.");
                return;
            }

            try
            {
                SoundPlayer player = new SoundPlayer(soundPath);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}");
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var task = TaskInputBox.Text.Trim();
            if (!string.IsNullOrEmpty(task))
            {
                TasksListBox.Items.Add(task);
                TaskInputBox.Text = "";
            }
        }
    }
}
