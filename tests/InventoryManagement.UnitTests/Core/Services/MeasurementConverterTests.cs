using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Services;

namespace InventoryManagement.UnitTests.Core.Services;

public class MeasurementConverterTests
{
  [Theory]
  [InlineData(1000, "Grams", "Kilograms", 1)]
  [InlineData(1, "Kilograms", "Grams", 1000)]
  [InlineData(500, "Milliliters", "Liters", 0.5)]
  [InlineData(1.5, "Liters", "Milliliters", 1500)]
  [InlineData(10, "Unit", "Unit", 10)]
  public void Convert_ShouldReturnCorrectValue_WhenConversionIsValid(double value, string fromUnitName,
    string toUnitName, double expectedValue)
  {
    var fromUnit = MeasurementUnit.FromName(fromUnitName);
    var toUnit = MeasurementUnit.FromName(toUnitName);

    var result = MeasurementConverter.Convert(value, fromUnit, toUnit);

    result.ShouldBe(expectedValue);
  }

  [Fact]
  public void Convert_ShouldThrow_WhenCategoriesAreDifferent()
  {
    var fromUnit = MeasurementUnit.Grams; // Mass
    var toUnit = MeasurementUnit.Liters; // Volume

    Should.Throw<ArgumentException>(() => MeasurementConverter.Convert(100, fromUnit, toUnit));
  }
}
