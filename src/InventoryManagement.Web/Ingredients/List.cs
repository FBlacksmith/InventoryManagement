using InventoryManagement.UseCases.Ingredients.List;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine;

namespace InventoryManagement.Web.Ingredients;

public class List(IMessageBus _bus) : Endpoint<ListIngredientsQuery, Results<Ok<IEnumerable<IngredientDTO>>, ProblemHttpResult>>
{
  public override void Configure()
  {
    Get("/ingredients");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "List ingredients";
      s.Description = "List ingredients with optional pagination";
      s.ExampleRequest = new ListIngredientsQuery(0, 10);
      s.ResponseExamples[200] = new[] { new IngredientDTO(Guid.NewGuid(), "Flour", "Grams") };
    });
    Tags("Ingredients");
  }

  public override async Task<Results<Ok<IEnumerable<IngredientDTO>>, ProblemHttpResult>> ExecuteAsync(ListIngredientsQuery req, CancellationToken ct)
  {
    var result = await _bus.InvokeAsync<Result<IEnumerable<IngredientDTO>>>(req, ct);

    if (result.IsSuccess)
    {
      return TypedResults.Ok(result.Value);
    }

    return TypedResults.Problem(detail: string.Join("; ", result.Errors), statusCode: 400);
  }
}
