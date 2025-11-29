using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data.Entities;
using CulinaryCommand.Inventory.Entities;

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
        public DbSet<CulinaryCommand.Inventory.Entities.Ingredient> Ingredients => Set<CulinaryCommand.Inventory.Entities.Ingredient>();
        public DbSet<MeasurementUnit> MeasurementUnits => Set<MeasurementUnit>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();
        public DbSet<UserLocation> UserLocations => Set<UserLocation>();
        public DbSet<ManagerLocation> ManagerLocations => Set<ManagerLocation>();
        public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
        public DbSet<CulinaryCommand.Inventory.Entities.Unit> Units => Set<CulinaryCommand.Inventory.Entities.Unit>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

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

            // Explicit join: Employees
            modelBuilder.Entity<UserLocation>()
                .HasKey(ul => new { ul.UserId, ul.LocationId });

            modelBuilder.Entity<UserLocation>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLocations)
                .HasForeignKey(ul => ul.UserId);

            modelBuilder.Entity<UserLocation>()
                .HasOne(ul => ul.Location)
                .WithMany(l => l.UserLocations)
                .HasForeignKey(ul => ul.LocationId);


            // Explicit join: Managers 
            modelBuilder.Entity<ManagerLocation>()
                .HasKey(ml => new { ml.UserId, ml.LocationId });

            modelBuilder.Entity<ManagerLocation>()
                .HasOne(ml => ml.User)
                .WithMany(u => u.ManagerLocations)
                .HasForeignKey(ml => ml.UserId);

            modelBuilder.Entity<ManagerLocation>()
                .HasOne(ml => ml.Location)
                .WithMany(l => l.ManagerLocations)
                .HasForeignKey(ml => ml.LocationId);
                
        }
    }
}