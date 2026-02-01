using Ardalis.Result;
using Ardalis.SharedKernel;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Ingredients.Specifications;
using InventoryManagement.Core.ProductionOrders;
using InventoryManagement.Core.Recipes;
using InventoryManagement.Core.Recipes.Specifications;

namespace InventoryManagement.UseCases.ProductionOrders.Create;

public class CreateProductionOrderHandler(
    IRepository<Recipe> _recipeRepository,
    IRepository<Ingredient> _ingredientRepository,
    IRepository<ProductionOrder> _orderRepository)
{
  public async Task<Result<ProductionOrderId>> Handle(CreateProductionOrderCommand command, CancellationToken cancellationToken)
  {
    if (command.Quantity <= 0)
    {
      return Result.Invalid(new ValidationError("Quantity", "validation.positive_quantity_required", "InvalidQuantity", ValidationSeverity.Error));
    }

    var recipeSpec = new RecipeWithIngredientsSpec(RecipeId.From(command.RecipeId));
    var recipe = await _recipeRepository.FirstOrDefaultAsync(recipeSpec, cancellationToken);

    if (recipe == null)
    {
      return Result.NotFound();
    }

    // 1. Load all required ingredients
    var ingredientIds = recipe.Ingredients.Select(x => x.IngredientId).Distinct().ToList();
    var ingredientsSpec = new IngredientsByIdsSpec(ingredientIds);
    var ingredients = await _ingredientRepository.ListAsync(ingredientsSpec, cancellationToken);


    // 2. Validate Stock
    foreach (var recipeIngredient in recipe.Ingredients)
    {
      var ingredient = ingredients.FirstOrDefault(i => i.Id == recipeIngredient.IngredientId);
      if (ingredient == null)
      {
        return Result.Invalid(new ValidationError("Ingredients", "validation.ingredient_not_found", "MissingIngredient", ValidationSeverity.Error));
      }

      double requiredAmount = recipeIngredient.Quantity * command.Quantity;
      if (ingredient.CurrentStock < requiredAmount)
      {
        return Result.Invalid(new ValidationError("Stock",
            "validation.insufficient_stock",
            "InsufficientStock",
            ValidationSeverity.Error));
      }
    }

    // 3. Create Order
    var order = new ProductionOrder(ProductionOrderId.From(Guid.NewGuid()), recipe.Id, command.Quantity, DateTime.UtcNow);

    // 4. Calculate Cost
    var estimatedCost = recipe.CalculateEstimatedCost(ingredients) * (decimal)command.Quantity;
    order.SetEstimatedCost(estimatedCost);

    // 5. Reserve (Status Change)
    order.ReserveIngredients();

    // 6. Persist
    await _orderRepository.AddAsync(order, cancellationToken);

    return order.Id;
  }
}
