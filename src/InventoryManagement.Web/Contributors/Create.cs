using System.ComponentModel.DataAnnotations;
using FluentValidation;
using InventoryManagement.Core.ContributorAggregate;
using InventoryManagement.UseCases.Contributors.Create;
using InventoryManagement.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine;

namespace InventoryManagement.Web.Contributors;

// This shows an example of having all related types in one file for simplicity.
// Fast-Endpoints generally uses one file per class for larger projects, which
// is the recommended approach. More files, but fewer merge conflicts and easier to 
// see what changed in a given commit or PR.

public class Create(IMessageBus _bus)
  : Endpoint<CreateContributorRequest,
    Results<Created<CreateContributorResponse>,
      ValidationProblem,
      ProblemHttpResult>>
{
  // The private field _mediator is replaced by _bus, matching the constructor parameter.
  // private readonly IMediator _mediator = mediator; // This line is removed/changed
  // The constructor parameter _bus is directly used, so a separate private field for it is not strictly necessary if it's only used once,
  // but if it were a different name, it would be: private readonly IMessageBus _bus = bus;
  // Given the provided edit, the constructor parameter is named `_bus`, so it can be used directly.

  public override void Configure()
  {
    Post(CreateContributorRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Create a new contributor";
      s.Description =
        "Creates a new contributor with the provided name. The contributor name must be between 2 and 100 characters long.";
      s.ExampleRequest = new CreateContributorRequest { Name = "John Doe" };
      s.ResponseExamples[201] = new CreateContributorResponse(1, "John Doe");

      // Document possible responses
      s.Responses[201] = "Contributor created successfully";
      s.Responses[400] = "Invalid input data - validation errors";
      s.Responses[500] = "Internal server error";
    });

    // Add tags for API grouping
    Tags("Contributors");

    // Add additional metadata
    Description(builder => builder
      .Accepts<CreateContributorRequest>("application/json")
      .Produces<CreateContributorResponse>(201, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(500));
  }

  public override async Task<Results<Created<CreateContributorResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateContributorRequest request, CancellationToken cancellationToken)
  {
    var result = await _bus.InvokeAsync<Result<ContributorId>>(
      new CreateContributorCommand(ContributorName.From(request.Name!), request.PhoneNumber),
      cancellationToken);

    return result.ToCreatedResult(
      id => $"/Contributors/{id}",
      id => new CreateContributorResponse(id.Value, request.Name!));
  }
}

public class CreateContributorRequest
{
  public const string Route = "/Contributors";

  [Required] public string Name { get; set; } = String.Empty;
  public string? PhoneNumber { get; set; } = null;
}

public class CreateContributorValidator : Validator<CreateContributorRequest>
{
  public CreateContributorValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MinimumLength(2)
      .MaximumLength(ContributorName.MaxLength);
  }
}

public class CreateContributorResponse(int id, string name)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
}
