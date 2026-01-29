using InventoryManagement.Infrastructure.Data;
using InventoryManagement.Web.Configurations;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults() // This sets up OpenTelemetry logging
  .AddLoggerConfigs(); // This adds Serilog for console formatting

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Starting web host");

builder.Services.AddOptionConfigs(builder.Configuration, startupLogger, builder);
builder.Services.AddServiceConfigs(startupLogger, builder);

builder.Host.UseWolverine(options =>
{
  options.Discovery.IncludeAssembly(typeof(InventoryManagement.UseCases.Contributors.Create.CreateContributorHandler)
    .Assembly);
  options.UseEntityFrameworkCoreTransactions();
  options.PersistMessagesWithSqlServer(builder.Configuration.GetConnectionString("cleanarchitecture")!);
});

builder.Services.AddFastEndpoints()
  .SwaggerDocument(o =>
  {
    o.ShortSchemaNames = true;
  });

var app = builder.Build();

await app.UseAppMiddlewareAndSeedDatabase();

app.MapDefaultEndpoints(); // Aspire health checks and metrics

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
