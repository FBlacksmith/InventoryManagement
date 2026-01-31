using InventoryManagement.Core.Enums;

namespace InventoryManagement.UnitTests.Core.Enums;

public class MeasurementUnitTests
{
  [Theory]
  [InlineData("Grams", 1, "G", MeasurementCategory.Mass)]
  [InlineData("Kilograms", 2, "KG", MeasurementCategory.Mass)]
  [InlineData("Milliliters", 3, "ML", MeasurementCategory.Volume)]
  [InlineData("Liters", 4, "L", MeasurementCategory.Volume)]
  [InlineData("Unit", 5, "UN", MeasurementCategory.Count)]
  public void MeasurementUnit_ShouldHaveCorrectValues(string name, int value, string symbol,
    MeasurementCategory category)
  {
    var unit = MeasurementUnit.FromName(name);

    unit.Value.ShouldBe(value);
    unit.Symbol.ShouldBe(symbol);
    unit.Category.ShouldBe(category);
  }

  [Fact]
  public void MeasurementUnit_ShouldThrow_WhenNameIsInvalid()
  {
    Should.Throw<Ardalis.SmartEnum.SmartEnumNotFoundException>(() => MeasurementUnit.FromName("Invalid"));
  }
}
