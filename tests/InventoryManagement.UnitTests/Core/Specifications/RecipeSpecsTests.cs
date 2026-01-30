using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Recipes;
using InventoryManagement.Core.Recipes.Specifications;

namespace InventoryManagement.UnitTests.Core.Specifications;

public class RecipeSpecsTests
{
  [Fact]
  public void RecipeWithIngredientsSpec_ShouldIncludeIngredients()
  {
    // Setup
    var r1Id = RecipeId.From(Guid.NewGuid());
    var i1Id = IngredientId.From(Guid.NewGuid());

    // Spec check is usually done via integration or evaluator. 
    // Here we just check it doesn't crash construction.
    var spec = new RecipeWithIngredientsSpec(r1Id);

    spec.WhereExpressions.ShouldNotBeEmpty();
    spec.IncludeExpressions.ShouldNotBeEmpty();
  }
}
