using InventoryManagement.Core.Recipes;

namespace InventoryManagement.Infrastructure.Data.Config;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
  public void Configure(EntityTypeBuilder<Recipe> builder)
  {
    builder.HasKey(r => r.Id);

    builder.Property(r => r.Id)
      .HasConversion(
        id => id.Value,
        value => RecipeId.From(value))
      .ValueGeneratedOnAdd();

    builder.Property(r => r.Name)
      .HasMaxLength(100)
      .IsRequired();

    builder.HasMany(r => r.Ingredients)
      .WithOne()
      .HasForeignKey(ri => ri.RecipeId)
      .IsRequired();
  }
}
