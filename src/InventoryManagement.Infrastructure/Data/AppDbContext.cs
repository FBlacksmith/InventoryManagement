using InventoryManagement.Core.ContributorAggregate;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Recipes;

namespace InventoryManagement.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Ingredient> Ingredients => Set<Ingredient>();
  public DbSet<Recipe> Recipes => Set<Recipe>();
  public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override int SaveChanges() =>
    SaveChangesAsync().GetAwaiter().GetResult();
}
