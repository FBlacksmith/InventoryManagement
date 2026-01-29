namespace InventoryManagement.Core.Enums;

public class ProductionStatus : SmartEnum<ProductionStatus>
{
  public static readonly ProductionStatus Draft = new(nameof(Draft), 1);
  public static readonly ProductionStatus Reserved = new(nameof(Reserved), 2);
  public static readonly ProductionStatus Completed = new(nameof(Completed), 3);
  public static readonly ProductionStatus Cancelled = new(nameof(Cancelled), 4);

  private ProductionStatus(string name, int value) : base(name, value)
  {
  }
}
