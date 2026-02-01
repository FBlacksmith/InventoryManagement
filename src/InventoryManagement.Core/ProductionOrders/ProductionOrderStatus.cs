using Ardalis.SmartEnum;

namespace InventoryManagement.Core.ProductionOrders;

public class ProductionOrderStatus : SmartEnum<ProductionOrderStatus>
{
  public static readonly ProductionOrderStatus Created = new(nameof(Created), 1);
  public static readonly ProductionOrderStatus Reserved = new(nameof(Reserved), 2);
  public static readonly ProductionOrderStatus Completed = new(nameof(Completed), 3);
  public static readonly ProductionOrderStatus Cancelled = new(nameof(Cancelled), 4);

  private ProductionOrderStatus(string name, int value) : base(name, value)
  {
  }
}
