using InventoryManagement.Core.ContributorAggregate;
using InventoryManagement.Core.ContributorAggregate.Events;
using InventoryManagement.Core.Interfaces;

namespace InventoryManagement.Core.Services;

/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
/// <param name="_repository"></param>
/// <param name="_mediator"></param>
/// <param name="_logger"></param>
public class DeleteContributorService(
  IRepository<Contributor> _repository,
  IDomainEventDispatcher _dispatcher,
  ILogger<DeleteContributorService> _logger) : IDeleteContributorService
{
  public async ValueTask<Result> DeleteContributor(ContributorId contributorId)
  {
    _logger.LogInformation("Deleting Contributor {contributorId}", contributorId);
    Contributor? aggregateToDelete = await _repository.GetByIdAsync(contributorId);
    if (aggregateToDelete == null) return Result.NotFound();

    await _repository.DeleteAsync(aggregateToDelete);
    var domainEvent = new ContributorDeletedEvent(contributorId);
    await _dispatcher.DispatchAndClearEvents(new[]
    {
      aggregateToDelete
    }); // DispatchAndClearEvents expects entities, but here we created an event manually. 
    // Wait, IDomainEventDispatcher typically dispatches events attached to entities.
    // If I want to publish a standalone event, I might need to add it to the entity first.
    // aggregateToDelete.RegisterDomainEvent(domainEvent);
    // await _dispatcher.DispatchAndClearEvents(new[] { aggregateToDelete });

    return Result.Success();
  }
}
