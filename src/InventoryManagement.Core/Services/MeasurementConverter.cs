using InventoryManagement.Core.Enums;

namespace InventoryManagement.Core.Services;

public static class MeasurementConverter
{
  public static double Convert(double value, MeasurementUnit fromUnit, MeasurementUnit toUnit)
  {
    if (fromUnit == toUnit)
    {
      return value;
    }

    if (fromUnit.Category != toUnit.Category)
    {
      throw new ArgumentException(
        $"Cannot convert from {fromUnit.Name} ({fromUnit.Category}) to {toUnit.Name} ({toUnit.Category})");
    }

    // Normalize to base unit (Grams for Mass, Milliliters for Volume, Unit for Count)
    double baseValue = ToBaseUnit(value, fromUnit);

    // Convert from base unit to target unit
    return FromBaseUnit(baseValue, toUnit);
  }

  private static double ToBaseUnit(double value, MeasurementUnit unit) => unit switch
  {
    _ when unit == MeasurementUnit.Grams => value,
    _ when unit == MeasurementUnit.Kilograms => value * 1000,
    _ when unit == MeasurementUnit.Milliliters => value,
    _ when unit == MeasurementUnit.Liters => value * 1000,
    _ when unit == MeasurementUnit.Unit => value,
    _ => throw new NotSupportedException($"Unit {unit.Name} is not supported.")
  };

  private static double FromBaseUnit(double baseValue, MeasurementUnit unit) => unit switch
  {
    _ when unit == MeasurementUnit.Grams => baseValue,
    _ when unit == MeasurementUnit.Kilograms => baseValue / 1000,
    _ when unit == MeasurementUnit.Milliliters => baseValue,
    _ when unit == MeasurementUnit.Liters => baseValue / 1000,
    _ when unit == MeasurementUnit.Unit => baseValue,
    _ => throw new NotSupportedException($"Unit {unit.Name} is not supported.")
  };
}
