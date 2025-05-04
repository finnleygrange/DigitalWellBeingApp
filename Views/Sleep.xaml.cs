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

        private void SaveSleepButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select valid start and end dates.");
                return;
            }

            try
            {
                var startDate = StartDatePicker.SelectedDate.Value;
                var endDate = EndDatePicker.SelectedDate.Value;

                int startHour = int.Parse(((ComboBoxItem)StartHourBox.SelectedItem)?.Content.ToString() ?? "0");
                int startMinute = int.Parse(((ComboBoxItem)StartMinuteBox.SelectedItem)?.Content.ToString() ?? "0");
                int endHour = int.Parse(((ComboBoxItem)EndHourBox.SelectedItem)?.Content.ToString() ?? "0");
                int endMinute = int.Parse(((ComboBoxItem)EndMinuteBox.SelectedItem)?.Content.ToString() ?? "0");

                DateTime start = new DateTime(startDate.Year, startDate.Month, startDate.Day, startHour, startMinute, 0);
                DateTime end = new DateTime(endDate.Year, endDate.Month, endDate.Day, endHour, endMinute, 0);

                if (end <= start)
                {
                    MessageBox.Show("End time must be after start time.");
                    return;
                }

                double duration = (end - start).TotalHours;

                if (DataContext is SleepViewModel vm)
                {
                    vm.AddSleepEntry(start, duration);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save entry: {ex.Message}");
            }
        }

    }
}
