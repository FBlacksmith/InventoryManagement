using InventoryManagement.Core.ContributorAggregate;
using InventoryManagement.Core.ContributorAggregate.Specifications;

namespace InventoryManagement.UseCases.Contributors.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetContributorHandler(IReadRepository<Contributor> _repository)
{
  public async ValueTask<Result<ContributorDto>> Handle(GetContributorQuery request,
    CancellationToken cancellationToken)
  {
    var spec = new ContributorByIdSpec(ContributorId.From(request.ContributorId));
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new ContributorDto(entity.Id, entity.Name, entity.PhoneNumber ?? PhoneNumber.Unknown);
  }
}
