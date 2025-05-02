using DigitalWellBeingApp.ViewModels;
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

namespace DigitalWellBeingApp.Views
{
    /// <summary>
    /// Interaction logic for Sleep.xaml
    /// </summary>
    public partial class Sleep : UserControl
    {
        public Sleep()
        {
            InitializeComponent();
            this.DataContext = new SleepViewModel();
        }

        private void btnDay_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnWeek_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnMonth_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnSixMonths_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
