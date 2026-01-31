using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;
using Shouldly;
using Xunit;

namespace InventoryManagement.UnitTests.Core.Ingredients;

public class IngredientTests
{
  [Fact]
  public void Constructor_ShouldInitializeCorrectly()
  {
    var id = IngredientId.From(Guid.NewGuid());
    var name = "Flour";
    var unit = MeasurementUnit.Grams;

    var ingredient = new Ingredient(id, name, unit);

    ingredient.Id.ShouldBe(id);
    ingredient.Name.ShouldBe(name);
    ingredient.MeasurementUnit.ShouldBe(unit);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_ShouldThrowException_WhenNameIsInvalid(string? invalidName)
  {
    var id = IngredientId.From(Guid.NewGuid());
    
    // Assuming EntityBase or GuardClauses throw ArgumentException/ArgumentNullException
    // If not standard, I'll have to adjust. But standard practice is Guard.Against.NullOrEmpty
    // Let me check if Ingredient.cs uses guards (I saw it uses EntityBase but not explicitly Guard in constructor in previous view_file, wait)
    // Re-checking Ingredient.cs content from previous turns.
    // Line 7: public Ingredient(IngredientId id, string name, MeasurementUnit measurementUnit) { Id = id; Name = name; MeasurementUnit = measurementUnit; }
    // It does basic assignment. 
    // Wait, the USER REQUEST says: "Valide se o construtor lança exceções corretamente via GuardClauses".
    // I need to UPDATE Ingredient.cs to USE GuardClauses if it doesn't already!
    // The view_file output (Step 9) showed:
    // Name = name; MeasurementUnit = measurementUnit; 
    // It did NOT use GuardClauses.
    // So I must Refactor Ingredient.cs first!
    
    // I will write the test assuming it throws, and then I will fix the implementation.
    // Or rather, since I am in "Execution" I should fix the implementation first or in parallel.
    
    Should.Throw<ArgumentException>(() => new Ingredient(id, invalidName!, MeasurementUnit.Grams));
  }

  [Fact]
  public void Constructor_ShouldThrowException_WhenMeasurementUnitIsNull()
  {
      var id = IngredientId.From(Guid.NewGuid());
      Should.Throw<ArgumentNullException>(() => new Ingredient(id, "Flour", null!));
  }
}
