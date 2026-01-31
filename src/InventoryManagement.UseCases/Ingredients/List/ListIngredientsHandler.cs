using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Ingredients.Specifications;

namespace InventoryManagement.UseCases.Ingredients.List;

public class ListIngredientsHandler(IRepository<Ingredient> _repository)
{
  public async ValueTask<Result<IEnumerable<IngredientDTO>>> Handle(ListIngredientsQuery query,
    CancellationToken cancellationToken)
  {
    var spec = new ListIngredientsSpec(query.Skip, query.Take);
    var ingredients = await _repository.ListAsync(spec, cancellationToken);
    
    var dtos = ingredients.Select(i => new IngredientDTO(i.Id.Value, i.Name, i.MeasurementUnit.Name));

    return Result.Success(dtos);
  }
}
