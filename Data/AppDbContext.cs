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
        public DbSet<Company> Companies => Set<Company>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.Company)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Many-to-many relationship: User <-> Location
            modelBuilder.Entity<User>()
                .HasMany(u => u.Locations)
                .WithMany(l => l.Users)
                .UsingEntity(j => j.ToTable("UserLocations"));
        }
    }  
}