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
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<MeasurementUnit> MeasurementUnits => Set<MeasurementUnit>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();


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
            // users WORKING at locations
            modelBuilder.Entity<User>()
                .HasMany(u => u.Locations)
                .WithMany(l => l.Users)
                .UsingEntity(j => j.ToTable("UserLocations"));
                                            // table of users who WORK at locations

            // many-to-many managers managing locations    
            modelBuilder.Entity<User>()
                .HasMany(u => u.ManagedLocations)
                .WithMany(l => l.Managers)
                .UsingEntity(j => j.ToTable("LocationManagers")); 
                                            // table of users (managers) who MANAGE locations
        }
    }  
}