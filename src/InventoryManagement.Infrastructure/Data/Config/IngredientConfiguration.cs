using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.Infrastructure.Data.Config;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
  public void Configure(EntityTypeBuilder<Ingredient> builder)
  {
    builder.HasKey(i => i.Id);

    builder.Property(i => i.Id)
      .HasConversion(
        id => id.Value,
        value => IngredientId.From(value))
      .ValueGeneratedOnAdd();

    builder.Property(i => i.Name)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(i => i.MeasurementUnit)
      .HasConversion(
        u => u.Value,
        value => MeasurementUnit.FromValue(value));

    builder.Property(i => i.WeightedAverageCost)
      .HasPrecision(18, 4);
  }
}
