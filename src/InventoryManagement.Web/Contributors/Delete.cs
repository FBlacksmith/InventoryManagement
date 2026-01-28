using InventoryManagement.Core.ContributorAggregate;
using InventoryManagement.UseCases.Contributors.Delete;
using InventoryManagement.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Ardalis.Result; // Assuming Ardalis.Result is used for the Result type
using Wolverine;

namespace InventoryManagement.Web.Contributors;

public class Delete(IMessageBus _bus)
  : Endpoint<DeleteContributorRequest,
    Results<NoContent,
      NotFound,
      ProblemHttpResult>>
{
  public override void Configure()
  {
    Delete(DeleteContributorRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Delete a contributor";
      s.Description = "Deletes an existing contributor by ID. This action cannot be undone.";
      s.ExampleRequest = new DeleteContributorRequest { ContributorId = 1 };

      // Document possible responses
      s.Responses[204] = "Contributor deleted successfully";
      s.Responses[404] = "Contributor not found";
      s.Responses[400] = "Invalid request or deletion failed";
    });

    // Add tags for API grouping
    Tags("Contributors");

    // Add additional metadata
    Description(builder => builder
      .Accepts<DeleteContributorRequest>()
      .Produces(204)
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<NoContent, NotFound, ProblemHttpResult>>
    ExecuteAsync(DeleteContributorRequest req, CancellationToken ct)
  {
    var result = await _bus.InvokeAsync<Result>(new DeleteContributorCommand(req.ContributorId), ct);

    return result.ToDeleteResult();
  }
}
