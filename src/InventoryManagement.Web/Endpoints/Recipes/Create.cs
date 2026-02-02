using FastEndpoints;
using InventoryManagement.UseCases.Recipes.Create;
using Wolverine;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Web.Endpoints.Recipes;

public class Create(IMessageBus bus) : Endpoint<CreateRecipeRequest, CreateRecipeResponse>
{
    public override void Configure()
    {
        Post("/recipes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateRecipeRequest req, CancellationToken ct)
    {
        var command = new CreateRecipeCommand(req.Name, req.Ingredients);

        var result = await bus.InvokeAsync<Result<Core.Recipes.RecipeId>>(command, ct);

        if (result.IsSuccess)
        {
            HttpContext.Response.StatusCode = 200;
            await HttpContext.Response.WriteAsJsonAsync(new CreateRecipeResponse(result.Value.Value), cancellationToken: ct);
            return;
        }

        if (result.Status == ResultStatus.Invalid)
        {
            foreach (var error in result.ValidationErrors)
            {
                AddError(error.ErrorMessage);
            }
            ThrowIfAnyErrors(); 
        }

        if (result.Status == ResultStatus.NotFound)
        {
            await HttpContext.Response.WriteAsJsonAsync(Results.NotFound(), cancellationToken: ct);
            return;
        }

        // Fallback
        await HttpContext.Response.WriteAsJsonAsync(Results.BadRequest(), cancellationToken: ct);

    }
}

public class CreateRecipeRequest
{
    public string Name { get; set; } = string.Empty;
    public List<CreateRecipeIngredientDto> Ingredients { get; set; } = new();
}

public record CreateRecipeResponse(Guid Id);
