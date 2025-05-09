using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWellBeingApp.Models
{
    internal class PomodoroData
    {
        public int Id { get; set; } 
        public int CompletedPomodoros { get; set; }
        public DateTime Date { get; set; }
    }
}
