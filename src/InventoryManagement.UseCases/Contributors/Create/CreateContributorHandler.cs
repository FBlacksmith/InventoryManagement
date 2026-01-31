using InventoryManagement.Core.ContributorAggregate;

namespace InventoryManagement.UseCases.Contributors.Create;

public class CreateContributorHandler(IRepository<Contributor> _repository)
{
  public async ValueTask<Result<ContributorId>> Handle(CreateContributorCommand command,
    CancellationToken cancellationToken)
  {
    var newContributor = new Contributor(command.Name);
    if (!string.IsNullOrEmpty(command.PhoneNumber))
    {
      var phoneNumber = new PhoneNumber("+1", command.PhoneNumber, String.Empty);
      newContributor.UpdatePhoneNumber(phoneNumber);
    }

    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    return createdItem.Id;
  }
}
