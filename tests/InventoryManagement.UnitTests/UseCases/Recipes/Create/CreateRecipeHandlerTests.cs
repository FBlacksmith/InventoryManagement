using Ardalis.Result;
using Ardalis.Specification;
using InventoryManagement.Core.Recipes;
using InventoryManagement.UseCases.Recipes.Create;

namespace InventoryManagement.UnitTests.UseCases.Recipes.Create;

public class CreateRecipeHandlerTests
{
  private readonly IRepository<Recipe> _repository = Substitute.For<IRepository<Recipe>>();
  private readonly CreateRecipeHandler _handler;

  public CreateRecipeHandlerTests()
  {
    _handler = new CreateRecipeHandler(_repository);
  }

  [Fact]
  public async Task ReturnsError_WhenNameIsMissing()
  {
    var command = new CreateRecipeCommand("", new List<CreateRecipeIngredientDto>());

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.Invalid, result.Status);
    Assert.Contains(result.ValidationErrors, e => e.Identifier == "Name");
  }

  [Fact]
  public async Task ReturnsError_WhenRecipeAlreadyExists()
  {
    _repository.AnyAsync(Arg.Any<ISpecification<Recipe>>(), Arg.Any<CancellationToken>())
        .Returns(true);
    var command = new CreateRecipeCommand("Existing Recipe", new List<CreateRecipeIngredientDto>());

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.Invalid, result.Status);
    Assert.Contains(result.ValidationErrors, e => e.Identifier == "Recipe");
  }

  [Fact]
  public async Task ReturnsError_WhenIngredientsListIsEmpty()
  {
    _repository.AnyAsync(Arg.Any<ISpecification<Recipe>>(), Arg.Any<CancellationToken>())
        .Returns(false);
    var command = new CreateRecipeCommand("New Recipe", new List<CreateRecipeIngredientDto>());

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.Invalid, result.Status);
    Assert.Contains(result.ValidationErrors, e => e.Identifier == "Ingredients");
  }

  [Fact]
  public async Task CreatesRecipe_WhenDataIsValid()
  {
    _repository.AnyAsync(Arg.Any<ISpecification<Recipe>>(), Arg.Any<CancellationToken>())
        .Returns(false);

    var ingredients = new List<CreateRecipeIngredientDto>
        {
            new CreateRecipeIngredientDto(Guid.NewGuid(), 100)
        };
    var command = new CreateRecipeCommand("Valid Recipe", ingredients);

    var result = await _handler.Handle(command, CancellationToken.None);

    Assert.Equal(ResultStatus.Ok, result.Status);
    await _repository.Received(1).AddAsync(Arg.Any<Recipe>(), Arg.Any<CancellationToken>());
  }
}
