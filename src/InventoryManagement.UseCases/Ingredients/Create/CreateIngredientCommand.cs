using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.UseCases.Ingredients.Create;

public record CreateIngredientCommand(string Name, int MeasurementUnitId);
