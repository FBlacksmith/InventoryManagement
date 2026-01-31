using InventoryManagement.Core.Enums;

namespace InventoryManagement.Core.Ingredients;

public class Ingredient : EntityBase<Ingredient, IngredientId>, IAggregateRoot
{
  public Ingredient(IngredientId id, string name, MeasurementUnit measurementUnit)
  {
    Id = id;
    Name = name;
    MeasurementUnit = measurementUnit;
  }

  // EF Core constructor
  private Ingredient()
  {
    // Id is set by EF or defaults
    Name = null!;
    MeasurementUnit = null!;
  }

  public string Name { get; private set; }
  public MeasurementUnit MeasurementUnit { get; private set; }
  public double CurrentStock { get; private set; }
  public decimal WeightedAverageCost { get; private set; }

  public void AddStock(double quantity, decimal costPerUnit)
  {
    if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");
    if (costPerUnit < 0) throw new ArgumentOutOfRangeException(nameof(costPerUnit), "Cost cannot be negative.");

    var totalCurrentValue = (decimal)CurrentStock * WeightedAverageCost;
    var totalNewValue = (decimal)quantity * costPerUnit;

    CurrentStock += quantity;

    if (CurrentStock > 0)
    {
      WeightedAverageCost = (totalCurrentValue + totalNewValue) / (decimal)CurrentStock;
    }
  }

  public void ReduceStock(double quantity)
  {
    if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");
    if (CurrentStock < quantity) throw new InvalidOperationException("Not enough stock.");

    CurrentStock -= quantity;
  }
}
