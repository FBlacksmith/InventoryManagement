using Ardalis.Result;
using InventoryManagement.Core.ProductionOrders;
using Wolverine;

namespace InventoryManagement.UseCases.ProductionOrders.Create;

public record CreateProductionOrderCommand(Guid RecipeId, double Quantity) : ICommand<Result<ProductionOrderId>>;
