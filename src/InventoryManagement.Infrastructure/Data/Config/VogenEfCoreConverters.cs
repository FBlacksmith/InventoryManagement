using InventoryManagement.Core.ContributorAggregate;
using Vogen;

namespace InventoryManagement.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
