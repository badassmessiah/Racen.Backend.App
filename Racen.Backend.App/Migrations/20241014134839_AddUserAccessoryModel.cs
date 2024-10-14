using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Racen.Backend.App.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccessoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    AccelerationBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    AerodynamicsBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    TyreGripBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    FuelConsumptionBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    CarId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessories_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccessories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AccessoryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccessories_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccessories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_CarId_Type",
                table: "Accessories",
                columns: new[] { "CarId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccessories_AccessoryId",
                table: "UserAccessories",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccessories_UserId",
                table: "UserAccessories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccessories");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "Cars");
        }
    }
}
