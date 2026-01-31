using InventoryManagement.Core.ContributorAggregate;

namespace InventoryManagement.UseCases.Contributors.Update;

public class UpdateContributorHandler(IRepository<Contributor> _repository)
{
  public async ValueTask<Result<ContributorDto>> Handle(UpdateContributorCommand command,
    CancellationToken ct)
  {
    var existingContributor = await _repository.GetByIdAsync(ContributorId.From(command.ContributorId), ct);
    if (existingContributor == null)
    {
      return Result.NotFound();
    }

    existingContributor.UpdateName(ContributorName.From(command.NewName));

    await _repository.UpdateAsync(existingContributor, ct);

    return new ContributorDto(existingContributor.Id,
      existingContributor.Name, existingContributor.PhoneNumber ?? PhoneNumber.Unknown);
  }
}
