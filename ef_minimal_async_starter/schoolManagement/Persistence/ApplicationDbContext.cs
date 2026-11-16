using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.Entities;

namespace Persistence
{
    //NOTE: The solution initially does NOT build, but Persistence does!
    //With this, the dotnet ef commands will work.
    public partial class ApplicationDbContext() : DbContext
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

        //TODO 3 configure the model with the constraints
        public static void OnModelCreation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pupil>()
                .HasIndex(p => new { p.FirstName, p.LastName, p.BirthDate })
                .IsUnique();

            modelBuilder.Entity<SchoolClass>()
                .HasIndex(sc => new { sc.Name })
                .IsUnique();

            modelBuilder.Entity<SchoolLaptop>()
                .HasIndex(sl => new { sl.SerialNr })
                .IsUnique();

            modelBuilder.Entity<Subject>()
                .HasIndex(s => new { s.Title })
                .IsUnique();
            
            modelBuilder.Entity<Teacher>()
                .HasIndex(t  => new { t.FirstName, t.LastName })
                .IsUnique();
            
            //props
            modelBuilder.Entity<Pupil>()
                .Property(p => p.FirstName)
                .HasMaxLength(50);
            
            modelBuilder.Entity<Pupil>()
                .Property(p => p.LastName)
                .HasMaxLength(50);
        }
    }
}
