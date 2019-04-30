using MarsRover.Persistence.EFCore.Entities;
using MarsRover.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MarsRover.Persistence.EFCore.Context
{
    public class RoverContext : DbContext
    {
        //add-migration -Name V1 -OutputDir Migrations\MarsRoverDb -Context MarsRover.Persistence.EFCore.Context.RoverContext -Project MarsRover.Persistence.EFCore -StartupProject MarsRover.Persistence.EFCore
        internal DbSet<Plateau> Plateaus { get; set; }
        internal DbSet<Entities.Rover> Rovers { get; set; }
        public RoverContext(DbContextOptions<RoverContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plateau>().ToTable("Plateaus").HasKey(t => t.Id);
            modelBuilder.Entity<Plateau>().OwnsOne(t => t.Size);
            //modelBuilder.Entity<Plateau>().HasMany(t => t.Rovers).WithOne(t => t.Plateau);

            modelBuilder.Entity<Entities.Rover>().ToTable("Rovers");
            modelBuilder.Entity<Entities.Rover>().Property(t => t.IsLocked).IsConcurrencyToken();
            modelBuilder.Entity<Entities.Rover>().HasKey(t => new { t.Id });
            modelBuilder.Entity<Entities.Rover>().OwnsOne(t => t.Point);
            //modelBuilder.Entity<Entities.Rover>().HasOne(t => t.Plateau).WithMany(t => t.Rovers).HasForeignKey(t => t.PlateauId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var useRowNumberForPaging = ApplicationConfiguration.Instance.GetValue<int>("MSSQLVersion") < 2012;

                var migrationsAssembly = typeof(RoverContext).GetTypeInfo().Assembly.GetName().Name;
                optionsBuilder.UseSqlServer(
                    ApplicationConfiguration.Instance.GetValue<string>(
                        "MarsRoverContext:DatabaseConnectionString"),
                    builder => builder.EnableRetryOnFailure(3).CommandTimeout(60).MigrationsAssembly(migrationsAssembly).UseRowNumberForPaging(useRowNumberForPaging));
            }
        }
    }
}
