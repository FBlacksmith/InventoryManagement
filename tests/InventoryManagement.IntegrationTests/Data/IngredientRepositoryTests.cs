using InventoryManagement.Core.Ingredients;
using InventoryManagement.Core.Enums;
using InventoryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;
using Shouldly;

namespace InventoryManagement.IntegrationTests.Data;

public class IngredientRepositoryTests : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();
    private AppDbContext _dbContext = null!;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
            .Options;

        _dbContext = new AppDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _msSqlContainer.DisposeAsync();
    }

    [Fact]
    public async Task Add_ShouldPersistIngredient_And_GetById_ShouldRetrieveIt()
    {
        // Arrange
        var id = IngredientId.From(Guid.NewGuid());
        var name = "Flour";
        var unit = MeasurementUnit.Grams;
        var ingredient = new Ingredient(id, name, unit);
        var repository = new EfRepository<Ingredient>(_dbContext);

        // Act
        await repository.AddAsync(ingredient);
        
        // Clear tracker to verify persistence
        _dbContext.ChangeTracker.Clear();

        var retrievedIngredient = await repository.GetByIdAsync(id);

        // Assert
        retrievedIngredient.ShouldNotBeNull();
        retrievedIngredient.Id.ShouldBe(id);
        retrievedIngredient.Name.ShouldBe(name);
        retrievedIngredient.MeasurementUnit.ShouldBe(unit);
    }
}
