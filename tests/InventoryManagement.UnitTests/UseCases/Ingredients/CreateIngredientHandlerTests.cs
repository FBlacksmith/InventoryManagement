using Ardalis.Result;
using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.UseCases.Ingredients.Create;
using InventoryManagement.Core.Ingredients.Specifications;

namespace InventoryManagement.UnitTests.UseCases.Ingredients;

public class CreateIngredientHandlerTests
{
  private readonly IRepository<Ingredient> _repository;
  private readonly CreateIngredientHandler _handler;

  public CreateIngredientHandlerTests()
  {
    _repository = Substitute.For<IRepository<Ingredient>>();
    _handler = new CreateIngredientHandler(_repository);
  }

  [Fact]
  public async Task Handle_ShouldReturnSuccess_WhenMeasurementUnitIsValid()
  {
    // Arrange
    var command = new CreateIngredientCommand("Flour", MeasurementUnit.Grams.Value);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.ShouldBeTrue();
    // result.Value is a struct, so it cannot be null.
    await _repository.Received(1).AddAsync(Arg.Any<Ingredient>(), Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_ShouldReturnBeforeAdd_WhenMeasurementUnitIsInvalid()
  {
    // Arrange
    var command = new CreateIngredientCommand("Flour", 999); // Invalid ID

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.Invalid);
    await _repository.DidNotReceive().AddAsync(Arg.Any<Ingredient>(), Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_ShouldReturnInvalid_WhenIngredientAlreadyExists()
  {
    // Arrange
    var command = new CreateIngredientCommand("Flour", MeasurementUnit.Grams.Value);
    _repository.AnyAsync(Arg.Any<IngredientByNameAndUnitSpec>(), Arg.Any<CancellationToken>())
      .Returns(true);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsSuccess.ShouldBeFalse();
    result.Status.ShouldBe(ResultStatus.Invalid);
    result.ValidationErrors.ShouldContain(e => e.ErrorMessage == "An ingredient with this name and measurement unit already exists.");
    await _repository.DidNotReceive().AddAsync(Arg.Any<Ingredient>(), Arg.Any<CancellationToken>());
  }
}
