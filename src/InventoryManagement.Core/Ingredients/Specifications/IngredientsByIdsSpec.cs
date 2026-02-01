using Ardalis.Specification;

namespace InventoryManagement.Core.Ingredients.Specifications;

public class IngredientsByIdsSpec : Specification<Ingredient>
{
  public IngredientsByIdsSpec(IEnumerable<IngredientId> ids)
  {
    Query.Where(i => ids.Contains(i.Id));
  }
}
