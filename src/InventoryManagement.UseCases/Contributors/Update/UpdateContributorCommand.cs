using InventoryManagement.Core.ContributorAggregate;

namespace InventoryManagement.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
