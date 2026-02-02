using FastEndpoints;
using InventoryManagement.UseCases.ProductionOrders.Create;
using Wolverine;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using InventoryManagement.Core.ProductionOrders;

namespace InventoryManagement.Web.Endpoints.ProductionOrders;

public class Create(IMessageBus bus) : Endpoint<CreateProductionOrderRequest, CreateProductionOrderResponse>
{
    public override void Configure()
    {
        Post("/production-orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductionOrderRequest req, CancellationToken ct)
    {
        var command = new CreateProductionOrderCommand(req.RecipeId, req.Quantity);

        var result = await bus.InvokeAsync<Result<ProductionOrderId>>(command, ct);

        if (result.IsSuccess)
        {
            HttpContext.Response.StatusCode = 200;
            await HttpContext.Response.WriteAsJsonAsync(new CreateProductionOrderResponse(result.Value.Value), cancellationToken: ct);
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

        if (result.Status == ResultStatus.Error) // Insufficient stock etc might be Error or Invalid? Use case used Invalid for Stock.
        {
             await HttpContext.Response.WriteAsJsonAsync(Results.BadRequest(), cancellationToken: ct);
             return;
        }

        // Fallback
        await HttpContext.Response.WriteAsJsonAsync(Results.Problem(), cancellationToken: ct);
    }
}

public class CreateProductionOrderRequest
{
    public Guid RecipeId { get; set; }
    public double Quantity { get; set; }
}

public record CreateProductionOrderResponse(Guid Id);
