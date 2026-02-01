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

  public decimal CalculateEstimatedCost(IEnumerable<Ingredient> ingredients)
  {
      decimal totalCost = 0;
      foreach (var recipeIngredient in _ingredients)
      {
          var ingredient = ingredients.FirstOrDefault(i => i.Id == recipeIngredient.IngredientId);
          if (ingredient != null)
          {
              // Assumes RecipeIngredient.Quantity matches the Ingredient.MeasurementUnit context
              // If conversions are needed, they would go here. 
              // For now we assume simplistic matching units or normalized quantities.
              totalCost += ingredient.WeightedAverageCost * (decimal)recipeIngredient.Quantity;
          }
      }
      return totalCost;
  }

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
