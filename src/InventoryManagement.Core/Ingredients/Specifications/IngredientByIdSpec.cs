namespace InventoryManagement.Core.Ingredients.Specifications;

public sealed class IngredientByIdSpec : Specification<Ingredient>
{
  public IngredientByIdSpec(IngredientId id)
  {
    Query.Where(i => i.Id == id);
  }
}
