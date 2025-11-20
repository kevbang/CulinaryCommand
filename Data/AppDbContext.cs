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
        public DbSet<UserLocation> UserLocations => Set<UserLocation>();
        public DbSet<ManagerLocation> ManagerLocations => Set<ManagerLocation>();


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // ============================================================
    // COMPANY → USERS (1 → many)
    // A company can have many employees.
    // Users keep CompanyId; deleting a company should NOT delete users.
    // ============================================================
    modelBuilder.Entity<User>()
        .HasOne(u => u.Company)
        .WithMany(c => c.Employees)
        .HasForeignKey(u => u.CompanyId)
        .OnDelete(DeleteBehavior.Restrict);

    // ============================================================
    // COMPANY → LOCATIONS (1 → many)
    // A company owns many locations.
    // If a company is deleted, all its locations go too.
    // ============================================================
    modelBuilder.Entity<Location>()
        .HasOne(l => l.Company)
        .WithMany(c => c.Locations)
        .HasForeignKey(l => l.CompanyId)
        .OnDelete(DeleteBehavior.Cascade);

    // ============================================================
    // USER ↔ LOCATION via USERLOCATION (many-to-many employees)
    // ============================================================
    modelBuilder.Entity<UserLocation>()
        .HasKey(ul => new { ul.UserId, ul.LocationId });

    modelBuilder.Entity<UserLocation>()
        .HasOne(ul => ul.User)
        .WithMany(u => u.UserLocations)
        .HasForeignKey(ul => ul.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<UserLocation>()
        .HasOne(ul => ul.Location)
        .WithMany(l => l.UserLocations)
        .HasForeignKey(ul => ul.LocationId)
        .OnDelete(DeleteBehavior.Cascade);

    // ============================================================
    // USER ↔ LOCATION via MANAGERLOCATION (many-to-many managers)
    // ============================================================
    modelBuilder.Entity<ManagerLocation>()
        .HasKey(ml => new { ml.UserId, ml.LocationId });

    modelBuilder.Entity<ManagerLocation>()
        .HasOne(ml => ml.User)
        .WithMany(u => u.ManagerLocations)
        .HasForeignKey(ml => ml.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ManagerLocation>()
        .HasOne(ml => ml.Location)
        .WithMany(l => l.UserLocations)
        .HasForeignKey(ml => ml.LocationId)
        .OnDelete(DeleteBehavior.Cascade);

    // ============================================================
    // LOCATION → RECIPES (1 → many)
    // ============================================================
    modelBuilder.Entity<Recipe>()
        .HasOne(r => r.Location)
        .WithMany(l => l.Recipes)
        .HasForeignKey(r => r.LocationId)
        .OnDelete(DeleteBehavior.Cascade);

    // ============================================================
    // RECIPE ↔ INGREDIENT ↔ UNIT via RECIPEINGREDIENT
    // ============================================================
    modelBuilder.Entity<RecipeIngredient>()
        .HasOne(ri => ri.Recipe)
        .WithMany(r => r.RecipeIngredients)
        .HasForeignKey(ri => ri.RecipeId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<RecipeIngredient>()
        .HasOne(ri => ri.Ingredient)
        .WithMany(i => i.RecipeIngredients)
        .HasForeignKey(ri => ri.IngredientId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<RecipeIngredient>()
        .HasOne(ri => ri.Unit)
        .WithMany(u => u.RecipeIngredients)
        .HasForeignKey(ri => ri.UnitId)
        .OnDelete(DeleteBehavior.Restrict);
}
    }
}