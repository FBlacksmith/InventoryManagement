namespace InventoryManagement.Core.Enums;

public class MeasurementUnit : SmartEnum<MeasurementUnit>
{
  public static readonly MeasurementUnit Grams = new(nameof(Grams), 1, "G", MeasurementCategory.Mass);
  public static readonly MeasurementUnit Kilograms = new(nameof(Kilograms), 2, "KG", MeasurementCategory.Mass);
  public static readonly MeasurementUnit Milliliters = new(nameof(Milliliters), 3, "ML", MeasurementCategory.Volume);
  public static readonly MeasurementUnit Liters = new(nameof(Liters), 4, "L", MeasurementCategory.Volume);
  public static readonly MeasurementUnit Unit = new(nameof(Unit), 5, "UN", MeasurementCategory.Count);

  public string Symbol { get; }
  public MeasurementCategory Category { get; }

  private MeasurementUnit(string name, int value, string symbol, MeasurementCategory category)
    : base(name, value)
  {
    Symbol = symbol;
    Category = category;
  }
}
