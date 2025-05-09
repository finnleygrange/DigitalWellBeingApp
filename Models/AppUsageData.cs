using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWellBeingApp.Models
{
    public class AppUsageData
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public int TimeSpent { get; set; }
        public string TimeSpentFormatted
        {
            get
            {
                var time = TimeSpan.FromSeconds(TimeSpent);
                return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
            }
        }
        public int SessionCount { get; set; }
        public DateTime Date { get; set; }
    }
}
