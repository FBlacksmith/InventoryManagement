using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.Core.Recipes;

public class RecipeIngredient : EntityBase<RecipeIngredient, int>
{
  public RecipeIngredient(RecipeId recipeId, IngredientId ingredientId, double quantity)
  {
    RecipeId = recipeId;
    IngredientId = ingredientId;
    Quantity = quantity;
  }

  // EF Core constructor
  private RecipeIngredient() { }

  public RecipeId RecipeId { get; private set; }
  public IngredientId IngredientId { get; private set; }
  public double Quantity { get; private set; }

  // Navigation properties
  public Ingredient? Ingredient { get; private set; } // Nullable because it might not be loaded
}
