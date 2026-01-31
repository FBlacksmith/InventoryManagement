using InventoryManagement.Core.Ingredients;

namespace InventoryManagement.Core.Recipes;

public class Recipe : EntityBase<Recipe, RecipeId>, IAggregateRoot
{
  public Recipe(RecipeId id, string name)
  {
    Id = id;
    Name = name;
  }

  // EF Core constructor
  private Recipe()
  {
    Name = null!;
  }

  public string Name { get; private set; }

  // Initialized as empty list to avoid nulls
  private readonly List<RecipeIngredient> _ingredients = new();
  public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients.AsReadOnly();

  public void AddIngredient(IngredientId ingredientId, double quantity)
  {
    var existing = _ingredients.FirstOrDefault(x => x.IngredientId == ingredientId);
    if (existing != null)
    {
      throw new InvalidOperationException("Ingredient already exists in recipe.");
    }

    _ingredients.Add(new RecipeIngredient(this.Id, ingredientId, quantity));
  }

  public void RemoveIngredient(IngredientId ingredientId)
  {
    var ingredient = _ingredients.FirstOrDefault(x => x.IngredientId == ingredientId);
    if (ingredient != null)
    {
      _ingredients.Remove(ingredient);
    }
  }
}
