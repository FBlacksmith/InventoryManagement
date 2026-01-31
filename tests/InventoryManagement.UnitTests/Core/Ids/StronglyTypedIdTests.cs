using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.ProductionOrders;
using InventoryManagement.Core.Recipes;
using Vogen;

namespace InventoryManagement.UnitTests.Core.Ids;

public class StronglyTypedIdTests
{
  [Fact]
  public void IngredientId_ShouldThrow_WhenInitializedWithEmptyGuid()
  {
    Should.Throw<ValueObjectValidationException>(() => IngredientId.From(Guid.Empty));
  }

  [Fact]
  public void IngredientId_ShouldCreate_WhenInitializedWithValidGuid()
  {
    var guid = Guid.NewGuid();
    var id = IngredientId.From(guid);
    id.Value.ShouldBe(guid);
  }

  [Fact]
  public void RecipeId_ShouldThrow_WhenInitializedWithEmptyGuid()
  {
    Should.Throw<ValueObjectValidationException>(() => RecipeId.From(Guid.Empty));
  }

  [Fact]
  public void ProductionOrderId_ShouldThrow_WhenInitializedWithEmptyGuid()
  {
    Should.Throw<ValueObjectValidationException>(() => ProductionOrderId.From(Guid.Empty));
  }
}
