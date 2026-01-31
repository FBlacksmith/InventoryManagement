using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.UseCases.Ingredients.List;

public record IngredientDTO(Guid Id, string Name, string MeasurementUnit);
