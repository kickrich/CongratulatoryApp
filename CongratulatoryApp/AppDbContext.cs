using Microsoft.EntityFrameworkCore;
using System;

namespace CongratulatoryApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Record> birthdays { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=birthdays;Username=postgres;Password=1029");
        }
    }
}
