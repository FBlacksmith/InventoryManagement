using Ardalis.Specification;

namespace InventoryManagement.Core.Recipes.Specifications;

public class RecipeByNameSpec : Specification<Recipe>
{
  public RecipeByNameSpec(string name)
  {
    Query.Where(r => r.Name == name);
  }
}
