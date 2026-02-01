using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.UseCases.Ingredients.Create;

using InventoryManagement.Core.Ingredients.Specifications;

public class CreateIngredientHandler(IRepository<Ingredient> _repository)
{
  public async ValueTask<Result<IngredientId>> Handle(CreateIngredientCommand command,
    CancellationToken cancellationToken)
  {
    if (!MeasurementUnit.TryFromValue(command.MeasurementUnitId, out var unit))
    {
      return Result.Invalid(new ValidationError("MeasurementUnitId", "Invalid Measurement Unit ID", "InvalidUnit", ValidationSeverity.Error));
    }

    var spec = new IngredientByNameAndUnitSpec(command.Name, unit);
    if (await _repository.AnyAsync(spec, cancellationToken))
    {
      return Result.Invalid(new ValidationError("Ingredient", "An ingredient with this name and measurement unit already exists.", "DuplicateIngredient", ValidationSeverity.Error));
    }

    var newIngredient = new Ingredient(IngredientId.From(Guid.NewGuid()), command.Name, unit);

    await _repository.AddAsync(newIngredient, cancellationToken);

    return newIngredient.Id;
  }
}
