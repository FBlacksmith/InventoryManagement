using Ardalis.Result;
using Ardalis.Specification;
using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Ingredients.Specifications;
using InventoryManagement.Core.ProductionOrders;
using InventoryManagement.Core.Recipes;
using InventoryManagement.UseCases.ProductionOrders.Create;

namespace InventoryManagement.UnitTests.UseCases.ProductionOrders.Create;

public class CreateProductionOrderHandlerTests
{
  private readonly IRepository<Recipe> _recipeRepo = Substitute.For<IRepository<Recipe>>();
  private readonly IRepository<Ingredient> _ingredientRepo = Substitute.For<IRepository<Ingredient>>();
  private readonly IRepository<ProductionOrder> _orderRepo = Substitute.For<IRepository<ProductionOrder>>();
  private readonly CreateProductionOrderHandler _handler;

  public CreateProductionOrderHandlerTests()
  {
    _handler = new CreateProductionOrderHandler(_recipeRepo, _ingredientRepo, _orderRepo);
  }

  [Fact]
  public async Task ReturnsError_WhenQuantityIsZeroOrNegative()
  {
    var command = new CreateProductionOrderCommand(Guid.NewGuid(), 0);
    var result = await _handler.Handle(command, CancellationToken.None);
    Assert.Equal(ResultStatus.Invalid, result.Status);
    Assert.Contains(result.ValidationErrors, e => e.Identifier == "Quantity");
  }

  [Fact]
  public async Task ReturnsNotFound_WhenRecipeDoesNotExist()
  {
    _recipeRepo.FirstOrDefaultAsync(Arg.Any<ISpecification<Recipe>>(), Arg.Any<CancellationToken>())
        .Returns((Recipe?)null);

    var command = new CreateProductionOrderCommand(Guid.NewGuid(), 10);
    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.NotFound, result.Status);
  }

  [Fact]
  public async Task ReturnsError_WhenIngredientStockIsInsufficient()
  {
    var recipeId = RecipeId.From(Guid.NewGuid());
    var ingredientId = IngredientId.From(Guid.NewGuid());
    var recipe = new Recipe(recipeId, "Test Recipe");
    recipe.AddIngredient(ingredientId, 10); // Needs 10 per unit

    _recipeRepo.FirstOrDefaultAsync(Arg.Any<ISpecification<Recipe>>(), Arg.Any<CancellationToken>())
        .Returns(recipe);

    var ingredient = new Ingredient(ingredientId, "Test Ing", MeasurementUnit.Kilograms);
    ingredient.AddStock(50, 10.0m); // Stock 50
                                    // ListAsync mock
    _ingredientRepo.ListAsync(Arg.Any<IngredientsByIdsSpec>(), Arg.Any<CancellationToken>())
        .Returns(new List<Ingredient> { ingredient });

    // Order 10 units -> Needs 100 stock. Have 50.
    var command = new CreateProductionOrderCommand(recipeId.Value, 10);

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.Invalid, result.Status);
    Assert.Contains(result.ValidationErrors, e => e.Identifier == "Stock");
  }

  [Fact]
  public async Task CreatesOrder_WhenStockIsSufficient()
  {
    var recipeId = RecipeId.From(Guid.NewGuid());
    var ingredientId = IngredientId.From(Guid.NewGuid());
    var recipe = new Recipe(recipeId, "Test Recipe");
    recipe.AddIngredient(ingredientId, 10);

    _recipeRepo.FirstOrDefaultAsync(Arg.Any<ISpecification<Recipe>>(), Arg.Any<CancellationToken>())
        .Returns(recipe);

    var ingredient = new Ingredient(ingredientId, "Test Ing", MeasurementUnit.Kilograms);
    ingredient.AddStock(200, 10.0m); // Stock 200
    _ingredientRepo.ListAsync(Arg.Any<IngredientsByIdsSpec>(), Arg.Any<CancellationToken>())
        .Returns(new List<Ingredient> { ingredient });

    // Order 10 -> Needs 100. Have 200.
    var command = new CreateProductionOrderCommand(recipeId.Value, 10);

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.Ok, result.Status);
    await _orderRepo.Received(1).AddAsync(Arg.Any<ProductionOrder>(), Arg.Any<CancellationToken>());
  }
}
