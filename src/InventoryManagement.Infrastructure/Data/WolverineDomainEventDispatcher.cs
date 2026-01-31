using InventoryManagement.Core.Interfaces;
using Wolverine;

namespace InventoryManagement.Infrastructure.Data;

public class WolverineDomainEventDispatcher(IMessageBus messageBus) : IDomainEventDispatcher
{
  public async Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents)
  {
    foreach (var entity in entitiesWithEvents)
    {
      var events = entity.DomainEvents.ToArray();
      entity.ClearDomainEvents();
      foreach (var domainEvent in events)
      {
        await messageBus.PublishAsync(domainEvent).ConfigureAwait(false);
      }
    }
  }
}
