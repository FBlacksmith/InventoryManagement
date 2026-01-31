using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class RecipeAggregation : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "Ingredients",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
          MeasurementUnit = table.Column<int>(type: "int", nullable: false),
          CurrentStock = table.Column<double>(type: "float", nullable: false),
          WeightedAverageCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Ingredients", x => x.Id);
        });

    migrationBuilder.CreateTable(
        name: "Recipes",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Recipes", x => x.Id);
        });

    migrationBuilder.CreateTable(
        name: "RecipeIngredients",
        columns: table => new
        {
          Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
          RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          IngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
          Quantity = table.Column<double>(type: "float", nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
          table.ForeignKey(
                  name: "FK_RecipeIngredients_Ingredients_IngredientId",
                  column: x => x.IngredientId,
                  principalTable: "Ingredients",
                  principalColumn: "Id",
                  onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
                  name: "FK_RecipeIngredients_Recipes_RecipeId",
                  column: x => x.RecipeId,
                  principalTable: "Recipes",
                  principalColumn: "Id",
                  onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateIndex(
        name: "IX_RecipeIngredients_IngredientId",
        table: "RecipeIngredients",
        column: "IngredientId");

    migrationBuilder.CreateIndex(
        name: "IX_RecipeIngredients_RecipeId_IngredientId",
        table: "RecipeIngredients",
        columns: new[] { "RecipeId", "IngredientId" },
        unique: true);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "RecipeIngredients");

    migrationBuilder.DropTable(
        name: "Ingredients");

    migrationBuilder.DropTable(
        name: "Recipes");
  }
}
