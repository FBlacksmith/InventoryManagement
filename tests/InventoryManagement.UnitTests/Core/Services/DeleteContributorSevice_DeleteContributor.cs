using InventoryManagement.Core.Services;

namespace InventoryManagement.UnitTests.Core.Services;

public class DeleteContributorService_DeleteContributor
{
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private readonly IDomainEventDispatcher _dispatcher = Substitute.For<IDomainEventDispatcher>();
  private readonly ILogger<DeleteContributorService> _logger = Substitute.For<ILogger<DeleteContributorService>>();

  private readonly DeleteContributorService _service;

  public DeleteContributorService_DeleteContributor()
  {
    _service = new DeleteContributorService(_repository, _dispatcher, _logger);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenCantFindContributor()
  {
    int missingId = 9999;
    var result = await _service.DeleteContributor(ContributorId.From(missingId));

    result.Status.ShouldBe(Ardalis.Result.ResultStatus.NotFound);
  }
}
