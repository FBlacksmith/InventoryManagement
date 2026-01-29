using Vogen;

namespace InventoryManagement.Core.Recipes;

[ValueObject<Guid>]
public partial struct RecipeId
{
    private static Validation Validate(Guid value) =>
        value == Guid.Empty ? Validation.Invalid("Id cannot be empty.") : Validation.Ok;
}
