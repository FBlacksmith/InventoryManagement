using Ardalis.Specification;
using InventoryManagement.Core.Enums;

namespace InventoryManagement.Core.Ingredients.Specifications;

public class IngredientByNameAndUnitSpec : Specification<Ingredient>
{
  public IngredientByNameAndUnitSpec(string name, MeasurementUnit measurementUnit)
  {
    Query.Where(i => i.Name == name && i.MeasurementUnit == measurementUnit);
  }
}
