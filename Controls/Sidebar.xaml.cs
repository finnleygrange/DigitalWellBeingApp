using DigitalWellBeingApp.Views;
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

namespace DigitalWellBeingApp.Controls
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public static readonly RoutedEvent ViewDashboardEvent = EventManager.RegisterRoutedEvent(nameof(ViewDashboard), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Sidebar));
        public static readonly RoutedEvent ViewPomodoroEvent = EventManager.RegisterRoutedEvent(nameof(ViewPomodoro), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Sidebar));
        public static readonly RoutedEvent ViewReportsEvent = EventManager.RegisterRoutedEvent(nameof(ViewReports), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Sidebar));
        public static readonly RoutedEvent ViewSettingsEvent = EventManager.RegisterRoutedEvent(nameof(ViewSettings), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Sidebar));
        public static readonly RoutedEvent ViewSleepEvent = EventManager.RegisterRoutedEvent(nameof(ViewSleep), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Sidebar));

        public Sidebar()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler ViewDashboard
        {
            add { AddHandler(ViewDashboardEvent, value); }
            remove { RemoveHandler(ViewDashboardEvent, value); }
        }

        public event RoutedEventHandler ViewPomodoro
        {
            add { AddHandler(ViewPomodoroEvent, value); }
            remove { RemoveHandler(ViewPomodoroEvent, value); }
        }

        public event RoutedEventHandler ViewReports
        {
            add { AddHandler(ViewReportsEvent, value); }
            remove { RemoveHandler(ViewReportsEvent, value); }
        }

        public event RoutedEventHandler ViewSettings
        {
            add { AddHandler(ViewSettingsEvent, value); }
            remove { RemoveHandler(ViewSettingsEvent, value); }
        }

        public event RoutedEventHandler ViewSleep
        {
            add { AddHandler(ViewSleepEvent, value); }
            remove { RemoveHandler(ViewSleepEvent, value); }
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ViewDashboardEvent));
        }

        private void btnPomodoro_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ViewPomodoroEvent));
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ViewReportsEvent));
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ViewSettingsEvent));
        }

        private void btnSleep_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ViewSleepEvent));
        }
    }
}
