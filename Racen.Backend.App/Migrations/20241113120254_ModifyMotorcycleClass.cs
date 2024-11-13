using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Racen.Backend.App.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMotorcycleClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "FuelCapacity",
                table: "Motorcycles",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<decimal>(
                name: "Level",
                table: "Motorcycles",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Level",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchesPlayed",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Money",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelCapacity",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MatchesPlayed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "AspNetUsers");
        }
    }
}
