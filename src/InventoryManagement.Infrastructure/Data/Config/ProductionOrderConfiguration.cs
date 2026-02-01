using InventoryManagement.Core.ProductionOrders;
using InventoryManagement.Core.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Data.Config;

public class ProductionOrderConfiguration : IEntityTypeConfiguration<ProductionOrder>
{
  public void Configure(EntityTypeBuilder<ProductionOrder> builder)
  {
    builder.HasKey(p => p.Id);

    builder.Property(p => p.Id)
        .HasConversion(
            id => id.Value,
            value => ProductionOrderId.From(value));

    builder.Property(p => p.RecipeId)
        .HasConversion(
            id => id.Value,
            value => RecipeId.From(value));

    builder.Property(p => p.Quantity)
        .IsRequired();

    builder.Property(p => p.OrderDate)
        .IsRequired();

    builder.Property(p => p.Status)
        .HasConversion(
            p => p.Value,
            p => ProductionOrderStatus.FromValue(p))
        .IsRequired();
        
    builder.Property(p => p.EstimatedCost)
        .HasColumnType("decimal(18,2)");
  }
}
