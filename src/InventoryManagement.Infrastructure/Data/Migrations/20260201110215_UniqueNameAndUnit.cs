using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueNameAndUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name_MeasurementUnit",
                table: "Ingredients",
                columns: new[] { "Name", "MeasurementUnit" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name_MeasurementUnit",
                table: "Ingredients");
        }
    }
}
