using Ardalis.Result;
using InventoryManagement.Core.Recipes;
using Wolverine;

namespace InventoryManagement.UseCases.Recipes.Create;

public record CreateRecipeCommand(string Name, List<CreateRecipeIngredientDto> Ingredients) : ICommand<Result<RecipeId>>;

public record CreateRecipeIngredientDto(Guid IngredientId, double Quantity);
