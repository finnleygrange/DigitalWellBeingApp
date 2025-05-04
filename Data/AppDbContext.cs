using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigitalWellBeingApp.Models;

namespace DigitalWellBeingApp.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<SleepEntry> SleepEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=sleepdata.db");
    }
}
