using InventoryManagement.Core.ContributorAggregate.Events;
using InventoryManagement.Core.Interfaces;

namespace InventoryManagement.Core.ContributorAggregate.Handlers;

public class ContributorNameUpdatedEmailNotificationHandler(
  ILogger<ContributorDeletedHandler> logger,
  IEmailSender emailSender)
{
  public async ValueTask Handle(ContributorNameUpdatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Contributor Name Updated event for {contributorId}", domainEvent.Contributor.Id);

    await emailSender.SendEmailAsync("to@test.com",
      "from@test.com",
      $"Contributor {domainEvent.Contributor.Id} Name Updated",
      $"Contributor with id {domainEvent.Contributor.Id} had their name updated to {domainEvent.Contributor.Name}.");
  }
}
