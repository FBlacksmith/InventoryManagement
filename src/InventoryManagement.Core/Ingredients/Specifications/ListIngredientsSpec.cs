namespace InventoryManagement.Core.Ingredients.Specifications;

public sealed class ListIngredientsSpec : Specification<Ingredient>
{
  public ListIngredientsSpec(int? skip, int? take)
  {
    if (skip.HasValue)
    {
      Query.Skip(skip.Value);
    }

    if (take.HasValue)
    {
      Query.Take(take.Value);
    }
  }
}
