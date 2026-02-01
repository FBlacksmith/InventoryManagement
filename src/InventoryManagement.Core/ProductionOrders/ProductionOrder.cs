using Ardalis.SharedKernel;
using InventoryManagement.Core.Recipes;
using Ardalis.GuardClauses;

namespace InventoryManagement.Core.ProductionOrders;

public class ProductionOrder : EntityBase<ProductionOrder, ProductionOrderId>, IAggregateRoot
{
  public ProductionOrder(ProductionOrderId id, RecipeId recipeId, double quantity, DateTime orderDate)
  {
    Id = id;
    RecipeId = Guard.Against.Default(recipeId, nameof(recipeId));
    Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
    OrderDate = orderDate;
    Status = ProductionOrderStatus.Created;
  }

  // EF Core
  private ProductionOrder() 
  { 
      Status = null!;
  }

  public RecipeId RecipeId { get; private set; }
  public double Quantity { get; private set; }
  public ProductionOrderStatus Status { get; private set; }
  public DateTime OrderDate { get; private set; }
  public decimal EstimatedCost { get; private set; }

  public void ReserveIngredients()
  {
    if (Status != ProductionOrderStatus.Created)
    {
      throw new InvalidOperationException($"Cannot reserve ingredients for order with status {Status.Name}");
    }
    Status = ProductionOrderStatus.Reserved;
  }

  public void CompleteOrder()
  {
    if (Status != ProductionOrderStatus.Reserved)
    {
      throw new InvalidOperationException($"Cannot complete order with status {Status.Name}. Order must be reserved first.");
    }
    Status = ProductionOrderStatus.Completed;
  }

  public void CancelOrder()
  {
     if (Status == ProductionOrderStatus.Completed)
    {
      throw new InvalidOperationException("Cannot cancel a completed order.");
    }
    Status = ProductionOrderStatus.Cancelled;
  }

  public void SetEstimatedCost(decimal cost)
  {
      if (Status != ProductionOrderStatus.Created)
      {
           throw new InvalidOperationException("Cannot set cost after order is processed.");
      }
      EstimatedCost = Guard.Against.Negative(cost, nameof(cost));
  }
}
