using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.Entities;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<SchoolClass> SchoolClasses => Set<SchoolClass>();
        public DbSet<Pupil> Pupils => Set<Pupil>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration["ConnectionStrings:Default"]
                ?? throw new InvalidOperationException(
                    "Connection string 'ConnectionStrings:Default' is missing from appsettings.json.");

            optionsBuilder
                .UseNpgsql(connectionString)
                .LogTo(
                    message => Debug.WriteLine(message),
                    LogLevel.Debug,
                    DbContextLoggerOptions.SingleLine | DbContextLoggerOptions.UtcTime);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pupil>()
                .Property(p => p.BirthDate)
                .HasColumnType("date");
        }
    }
}
