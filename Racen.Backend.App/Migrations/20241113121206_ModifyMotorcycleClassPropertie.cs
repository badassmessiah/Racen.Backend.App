using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Racen.Backend.App.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMotorcycleClassPropertie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motorcycles_AspNetUsers_ApplicationUserId",
                table: "Motorcycles");

            migrationBuilder.DropIndex(
                name: "IX_Motorcycles_ApplicationUserId",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Motorcycles");

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_OwnerId",
                table: "Motorcycles",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Motorcycles_AspNetUsers_OwnerId",
                table: "Motorcycles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motorcycles_AspNetUsers_OwnerId",
                table: "Motorcycles");

            migrationBuilder.DropIndex(
                name: "IX_Motorcycles_OwnerId",
                table: "Motorcycles");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Motorcycles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_ApplicationUserId",
                table: "Motorcycles",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Motorcycles_AspNetUsers_ApplicationUserId",
                table: "Motorcycles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
