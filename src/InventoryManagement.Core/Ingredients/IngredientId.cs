using Vogen;

namespace InventoryManagement.Core.Ingredients;

[ValueObject<Guid>]
public partial struct IngredientId
{
    private static Validation Validate(Guid value) =>
        value == Guid.Empty ? Validation.Invalid("Id cannot be empty.") : Validation.Ok;
}
