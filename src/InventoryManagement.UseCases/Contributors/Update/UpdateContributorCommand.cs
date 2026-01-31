using InventoryManagement.Core.ContributorAggregate;

namespace InventoryManagement.UseCases.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName);
