using Ardalis.Specification;

namespace InventoryManagement.Core.Recipes.Specifications;

public class RecipeWithIngredientsSpec : Specification<Recipe>
{
  public RecipeWithIngredientsSpec(RecipeId recipeId)
  {
    Query
        .Where(r => r.Id == recipeId)
        .Include(r => r.Ingredients);
  }
}
