using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddProductionOrderAggregate : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "ProductionOrders",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          Quantity = table.Column<double>(type: "float", nullable: false),
          Status = table.Column<int>(type: "int", nullable: false),
          OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
          EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_ProductionOrders", x => x.Id);
        });
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "ProductionOrders");
  }
}
