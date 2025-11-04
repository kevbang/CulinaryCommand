using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data.Entities;

namespace CulinaryCommand.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Location> Locations => Set<Location>();
        public DbSet<User> Users => Set<User>();
        public DbSet<WorkTask> Tasks => Set<WorkTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set up many-to-many relationship between Users and Locations
            // This allows users to be associated with multiple locations and vice versa
            modelBuilder.Entity<User>()
                .HasMany(u => u.Locations)
                .WithMany(l => l.Users);
        }
    }  
}


