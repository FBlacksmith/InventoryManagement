using InventoryManagement.Core.ContributorAggregate;

namespace InventoryManagement.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
