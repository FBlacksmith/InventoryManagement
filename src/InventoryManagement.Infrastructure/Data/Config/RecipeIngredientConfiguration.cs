using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Recipes;

namespace InventoryManagement.Infrastructure.Data.Config;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
  public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
  {
    // Composite key or simple ID? The entity has 'int' ID in my previous step, but for a join table usually composite is better.
    // However, standard Entity<T> suggests a single ID.
    // Let's stick with the Entity<int> definition but enforce uniqueness on the pair.

    builder.ToTable("RecipeIngredients");

    builder.Property(x => x.Id).ValueGeneratedOnAdd();

    // Since it inherits Entity<int>, it has an Id property.
    // But for join purposes, we want to ensure RecipeId + IngredientId is unique.
    builder.HasIndex(ri => new { ri.RecipeId, ri.IngredientId }).IsUnique();

    builder.Property(ri => ri.RecipeId)
      .HasConversion(
        id => id.Value,
        value => RecipeId.From(value));

    builder.Property(ri => ri.IngredientId)
      .HasConversion(
        id => id.Value,
        value => IngredientId.From(value));

    builder.HasOne(ri => ri.Ingredient)
      .WithMany()
      .HasForeignKey(ri => ri.IngredientId);
  }
}
