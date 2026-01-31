using System.ComponentModel.DataAnnotations;
using FluentValidation;
using InventoryManagement.Core.Enums;
using InventoryManagement.Core.Ingredients;
using InventoryManagement.UseCases.Ingredients.Create;
using InventoryManagement.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine;

namespace InventoryManagement.Web.Ingredients;

public class Create(IMessageBus _bus)
  : Endpoint<CreateIngredientRequest,
    Results<Created<CreateIngredientResponse>,
      ValidationProblem,
      ProblemHttpResult>>
{
  public override void Configure()
  {
    Post(CreateIngredientRequest.Route);
    AllowAnonymous();
    var validUnits = string.Join(", ", MeasurementUnit.List.Select(u => u.Name));
    Summary(s =>
    {
      s.Summary = "Create a new ingredient";
      s.Description = $"Creates a new ingredient with the provided name and measurement unit. Valid measurement units are: {validUnits}.";
      s.ExampleRequest = new CreateIngredientRequest { Name = "Flour", MeasurementUnitName = "Grams" };
      s.ResponseExamples[201] = new CreateIngredientResponse(Guid.NewGuid(), "Flour");
      s.Responses[201] = "Ingredient created successfully";
      s.Responses[400] = "Invalid input data";
      s.Responses[500] = "Internal server error";
    });

    Tags("Ingredients");

    Description(builder => builder
      .Accepts<CreateIngredientRequest>("application/json")
      .Produces<CreateIngredientResponse>(201, "application/json")
      .ProducesProblem(400)
      .ProducesProblem(500));
  }

  public override async Task<Results<Created<CreateIngredientResponse>, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CreateIngredientRequest request, CancellationToken cancellationToken)
  {
    var measurementUnit = MeasurementUnit.FromName(request.MeasurementUnitName);
    var result = await _bus.InvokeAsync<Result<IngredientId>>(
      new CreateIngredientCommand(request.Name!, measurementUnit.Value),
      cancellationToken);

    return result.ToCreatedResult(
      id => $"/Ingredients/{id}",
      id => new CreateIngredientResponse(id.Value, request.Name!));
  }
}

public class CreateIngredientRequest
{
  public const string Route = "/Ingredients";

  [Required] public string Name { get; set; } = String.Empty;
  [Required] public string MeasurementUnitName { get; set; } = String.Empty;
}

public class CreateIngredientValidator : Validator<CreateIngredientRequest>
{
  public CreateIngredientValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.")
      .MaximumLength(100);

    RuleFor(x => x.MeasurementUnitName)
      .NotEmpty()
      .WithMessage("Measurement Unit Name is required.")
      .Must(name => MeasurementUnit.TryFromName(name, true, out _))
      .WithMessage($"Invalid Measurement Unit Name. Valid units are: {string.Join(", ", MeasurementUnit.List.Select(u => u.Name))}.");
  }
}

public class CreateIngredientResponse(Guid id, string name)
{
  public Guid Id { get; set; } = id;
  public string Name { get; set; } = name;
}
