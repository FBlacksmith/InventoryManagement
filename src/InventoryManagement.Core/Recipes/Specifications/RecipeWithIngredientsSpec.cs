namespace InventoryManagement.Core.Recipes.Specifications;

public class RecipeWithIngredientsSpec : Specification<Recipe>
{
  public RecipeWithIngredientsSpec(RecipeId id)
  {
    Query
      .Where(r => r.Id == id)
      .Include(r => r.Ingredients)
      .ThenInclude(ri => ri.Ingredient);
  }
}
