using Vogen;

namespace InventoryManagement.Core.ProductionOrders;

[ValueObject<Guid>]
public partial struct ProductionOrderId
{
  private static Validation Validate(Guid value) =>
      value == Guid.Empty ? Validation.Invalid("Id cannot be empty.") : Validation.Ok;
}
