using InventoryManagement.Core.ProductionOrders;
using InventoryManagement.Core.Recipes;
using Xunit;

namespace InventoryManagement.UnitTests.Core.ProductionOrders;

public class ProductionOrderTests
{
    private readonly RecipeId _recipeId = RecipeId.From(Guid.NewGuid());
    private readonly ProductionOrderId _orderId = ProductionOrderId.From(Guid.NewGuid());

    [Fact]
    public void IsCreatedWithCreatedStatus()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);

        Assert.Equal(ProductionOrderStatus.Created, order.Status);
    }

    [Fact]
    public void ReserveIngredients_TransitionsToReserved_WhenCreated()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);
        
        order.ReserveIngredients();

        Assert.Equal(ProductionOrderStatus.Reserved, order.Status);
    }

    [Fact]
    public void ReserveIngredients_ThrowsException_WhenAlreadyReserved()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);
        order.ReserveIngredients();

        Assert.Throws<InvalidOperationException>(() => order.ReserveIngredients());
    }

    [Fact]
    public void CompleteOrder_TransitionsToCompleted_WhenReserved()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);
        order.ReserveIngredients();

        order.CompleteOrder();

        Assert.Equal(ProductionOrderStatus.Completed, order.Status);
    }

    [Fact]
    public void CompleteOrder_ThrowsException_WhenCreated()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => order.CompleteOrder());
    }

    [Fact]
    public void CancelOrder_TransitionsToCancelled_WhenCreated()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);

        order.CancelOrder();

        Assert.Equal(ProductionOrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void CancelOrder_ThrowsException_WhenCompleted()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);
        order.ReserveIngredients();
        order.CompleteOrder();

        Assert.Throws<InvalidOperationException>(() => order.CancelOrder());
    }
    
    [Fact]
    public void SetEstimatedCost_UpdatesCost_WhenCreated()
    {
        var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);
        decimal cost = 50.0m;

        order.SetEstimatedCost(cost);

        Assert.Equal(cost, order.EstimatedCost);
    }

    [Fact]
    public void SetEstimatedCost_ThrowsException_WhenReserved()
    {
         var order = new ProductionOrder(_orderId, _recipeId, 10, DateTime.UtcNow);
         order.ReserveIngredients();

         Assert.Throws<InvalidOperationException>(() => order.SetEstimatedCost(10m));
    }
}
