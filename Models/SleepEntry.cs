using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWellBeingApp.Models
{
    class SleepEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double HoursSlept { get; set; }
    }
}
