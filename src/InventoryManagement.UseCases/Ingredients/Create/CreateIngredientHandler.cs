using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.UseCases.Ingredients.Create;

public class CreateIngredientHandler(IRepository<Ingredient> _repository)
{
  public async ValueTask<Result<IngredientId>> Handle(CreateIngredientCommand command,
    CancellationToken cancellationToken)
  {
    if (!MeasurementUnit.TryFromValue(command.MeasurementUnitId, out var unit))
    {
      return Result.Invalid(new ValidationError("MeasurementUnitId", "Invalid Measurement Unit ID", "InvalidUnit", ValidationSeverity.Error));
    }

    var newIngredient = new Ingredient(IngredientId.From(Guid.NewGuid()), command.Name, unit);

    await _repository.AddAsync(newIngredient, cancellationToken);

    return newIngredient.Id;
  }
}
