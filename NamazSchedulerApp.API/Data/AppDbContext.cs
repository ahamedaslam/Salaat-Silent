using Microsoft.EntityFrameworkCore;
using NamazSchedulerApp.API.Models;

namespace NamazSchedulerApp.API.Context

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<UserPreferences> Preferences { get; set; }

        public DbSet<Location> Locations {  get; set; }
        
        public DbSet<PrayerTimes> Prayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mark Location as a keyless entity
            modelBuilder.Entity<Location>().HasNoKey();
            modelBuilder.Entity<PrayerTimes>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}
