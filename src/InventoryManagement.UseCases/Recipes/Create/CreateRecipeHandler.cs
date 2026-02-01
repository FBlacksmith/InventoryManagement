using Ardalis.Result;
using Ardalis.SharedKernel;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Recipes;
using InventoryManagement.Core.Recipes.Specifications;

namespace InventoryManagement.UseCases.Recipes.Create;

public class CreateRecipeHandler(IRepository<Recipe> _repository)
{
  public async Task<Result<RecipeId>> Handle(CreateRecipeCommand command, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(command.Name))
    {
      return Result.Invalid(new ValidationError("Name", "validation.required", "Required", ValidationSeverity.Error));
    }

    var spec = new RecipeByNameSpec(command.Name);
    if (await _repository.AnyAsync(spec, cancellationToken))
    {
      return Result.Invalid(new ValidationError("Recipe", "validation.duplicate_recipe", "Duplicate", ValidationSeverity.Error));
    }

    // Checking if ingredients list is valid
    if (command.Ingredients is null || !command.Ingredients.Any())
    {
      return Result.Invalid(new ValidationError("Ingredients", "validation.recipe_needs_ingredients", "EmptyIngredients", ValidationSeverity.Error));
    }

    var newRecipe = new Recipe(RecipeId.From(Guid.NewGuid()), command.Name);

    foreach (var item in command.Ingredients)
    {
      try
      {
        // Note: In a real scenario we might want to validate if IngredientId exists here or let the DB constraints handle it / or aggregate checks.
        // For now, we assume IDs are valid or validated by frontend/lookup. 
        // Better approach: Load all ingredients to ensure they exist.
        newRecipe.AddIngredient(IngredientId.From(item.IngredientId), item.Quantity);
      }
      catch (Exception ex)
      {
        return Result.Invalid(new ValidationError("Ingredients", $"validation.error_adding_ingredient: {ex.Message}", "InvalidIngredient", ValidationSeverity.Error));
      }
    }

    await _repository.AddAsync(newRecipe, cancellationToken);

    return newRecipe.Id;
  }
}
