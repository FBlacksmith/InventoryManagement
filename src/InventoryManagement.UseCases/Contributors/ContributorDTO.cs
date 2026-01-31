using InventoryManagement.Core.ContributorAggregate;

namespace InventoryManagement.UseCases.Contributors;

public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
