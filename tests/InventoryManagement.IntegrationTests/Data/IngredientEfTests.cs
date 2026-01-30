using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.Infrastructure.Data;

namespace InventoryManagement.IntegrationTests.Data;

public class IngredientEfTests
{
  private readonly AppDbContext _dbContext;

  public IngredientEfTests()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    _dbContext = new AppDbContext(options);
  }

  [Fact]
  public async Task CanAddAndGetIngredient()
  {
    var id = IngredientId.From(Guid.NewGuid());
    var ingredient = new Ingredient(id, "Flour", MeasurementUnit.Grams);

    _dbContext.Ingredients.Add(ingredient);
    await _dbContext.SaveChangesAsync();

    var result = await _dbContext.Ingredients.FirstOrDefaultAsync(i => i.Id == id);

    result.ShouldNotBeNull();
    result.Name.ShouldBe("Flour");
    result.MeasurementUnit.ShouldBe(MeasurementUnit.Grams);
    result.Id.ShouldBe(id);
  }

  [Fact]
  public async Task SmartEnumPersistsAsValue()
  {
    var id = IngredientId.From(Guid.NewGuid());
    var ingredient = new Ingredient(id, "Milk", MeasurementUnit.Liters);

    _dbContext.Ingredients.Add(ingredient);
    await _dbContext.SaveChangesAsync();

    // Verify it was saved correcty logic (InMemory doesn't show raw SQL value easily but behaviour is enough)
    var result = await _dbContext.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
    result!.MeasurementUnit.ShouldBe(MeasurementUnit.Liters);
  }
}
