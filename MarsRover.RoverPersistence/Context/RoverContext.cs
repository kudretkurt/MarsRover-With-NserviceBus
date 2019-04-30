using MarsRover.RoverPersistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarsRover.RoverPersistence.Context
{
    public class RoverContext : DbContext
    {
        internal DbSet<Plateau> Plateaus { get; set; }
        internal DbSet<Entities.Rover> Rovers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plateau>().ToTable("Plateaus").HasKey(t => t.Id);
            modelBuilder.Entity<Plateau>().OwnsOne(t => t.Size);
            modelBuilder.Entity<Plateau>().HasMany(t => t.Rovers).WithOne(t => t.Plateau);

            modelBuilder.Entity<Entities.Rover>().ToTable("Rovers");
            modelBuilder.Entity<Entities.Rover>().HasKey(t => t.Id);
            modelBuilder.Entity<Entities.Rover>().OwnsOne(t => t.Point);
            modelBuilder.Entity<Entities.Rover>().HasOne(t => t.Plateau).WithMany(t => t.Rovers);
        }
    }
}
