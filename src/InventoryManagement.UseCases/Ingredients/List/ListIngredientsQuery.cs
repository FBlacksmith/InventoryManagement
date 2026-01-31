using Ardalis.Result;

namespace InventoryManagement.UseCases.Ingredients.List;

public record ListIngredientsQuery(int? Skip, int? Take);

