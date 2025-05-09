using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigitalWellBeingApp.Models;
using System.IO;

namespace DigitalWellBeingApp.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<SleepEntry> SleepEntries { get; set; }
        public DbSet<AppUsageData> AppUsageData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.db")}");
    }
}
