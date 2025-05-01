using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using DigitalWellBeingApp.Views;
using LiveCharts;
using LiveCharts.Wpf;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Diagnostics;

namespace DigitalWellBeingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private DispatcherTimer _timer;
        private string _currentAppTitle;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            //PointLabel = chartPoint =>
            //    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            //DataContext = this;
        }

        

        private void Timer_Tick(object sender, EventArgs e)
        {
            string title = GetActiveWindowTitle();

            if (title != _currentAppTitle)
            {
                _currentAppTitle = title;
                //TrackApplicationUsage(title);  
            }
        }

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        //private void TrackApplicationUsage(string appTitle)
        //{
        //    var timeSpent = DateTime.Now - _appStartTimes[appTitle];
        //    if (_appTimeSpent.ContainsKey(appTitle))
        //    {
        //        _appTimeSpent[appTitle] += timeSpent;
        //    }
        //    else
        //    {
        //        _appTimeSpent[appTitle] = timeSpent;
        //    }

        //    // Debugging: Output to the console
        //    Debug.WriteLine($"{appTitle} spent: {timeSpent.TotalSeconds} seconds");
        //}

















        //public Func<ChartPoint, string> PointLabel { get; set; }

        //private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        //{
        //    var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;

        //    //clear selected slice.
        //    foreach (PieSeries series in chart.Series)
        //        series.PushOut = 0;

        //    var selectedSeries = (PieSeries)chartPoint.SeriesView;
        //    selectedSeries.PushOut = 8;
        //}


        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Sidebar_ViewDashboard(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Dashboard();
        }

        private void Sidebar_ViewPomodoro(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Pomodoro();
        }

        private void Sidebar_ViewReports(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Reports();
        }

        private void Sidebar_ViewSettings(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Settings();
        }
    }
}