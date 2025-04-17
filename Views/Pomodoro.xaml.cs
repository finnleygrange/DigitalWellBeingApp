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
    /// <summary>
    /// Interaction logic for Pomodoro.xaml
    /// </summary>
    public partial class Pomodoro : UserControl
    {

        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;
        private bool _isRunning = false;

        public Pomodoro()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            _timeLeft = TimeSpan.FromMinutes(25);
            UpdateTimerDisplay();
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
            }
        }

        private void UpdateTimerDisplay()
        {
            TimerText.Text = _timeLeft.ToString(@"mm\:ss");
        }



        private void btnPomodoro_Checked(object sender, RoutedEventArgs e)
        {

            if (TimerText == null)
                return;

            _timeLeft = TimeSpan.FromMinutes(25);
            UpdateTimerDisplay();
            btnReset_Click(sender, e);
        }

        private void btnShortBreak_Checked(object sender, RoutedEventArgs e)
        {
            _timeLeft = TimeSpan.FromMinutes(5);
            UpdateTimerDisplay();
            btnReset_Click(sender, e);
        }
        private void btnLongBreak_Checked(object sender, RoutedEventArgs e)
        {
            _timeLeft = TimeSpan.FromMinutes(15);
            UpdateTimerDisplay();
            btnReset_Click(sender, e);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            _isRunning = false;
            _timer.Stop();
            btnStart.Content = "START";

            if (btnPomodoro.IsChecked == true)
            {
                _timeLeft = TimeSpan.FromMinutes(25);
            }
            else if (btnShortBreak.IsChecked == true)
            {
                _timeLeft = TimeSpan.FromMinutes(5);
            }
            else if (btnLongBreak.IsChecked == true)
            {
                _timeLeft = TimeSpan.FromMinutes(15);
            }

            UpdateTimerDisplay();
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
    }
}
