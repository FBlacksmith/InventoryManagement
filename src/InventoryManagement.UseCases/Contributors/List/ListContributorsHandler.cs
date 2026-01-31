namespace InventoryManagement.UseCases.Contributors.List;

public class ListContributorsHandler(IListContributorsQueryService _queryService)
{
  public async ValueTask<Result<PagedResult<ContributorDto>>> Handle(ListContributorsQuery request,
    CancellationToken cancellationToken)
  {
    var result = await _queryService.ListAsync(request.Page, request.PerPage);

    return Result.Success(result);
  }
}
