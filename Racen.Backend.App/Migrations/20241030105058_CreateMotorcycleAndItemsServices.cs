using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Racen.Backend.App.Migrations
{
    /// <inheritdoc />
    public partial class CreateMotorcycleAndItemsServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Motorcycles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Items");
        }
    }
}
