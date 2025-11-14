using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerLocationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "LocationManagers",
                columns: table => new
                {
                    ManagedLocationsId = table.Column<int>(type: "int", nullable: false),
                    ManagersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationManagers", x => new { x.ManagedLocationsId, x.ManagersId });
                    table.ForeignKey(
                        name: "FK_LocationManagers_Locations_ManagedLocationsId",
                        column: x => x.ManagedLocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationManagers_Users_ManagersId",
                        column: x => x.ManagersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LocationManagers_ManagersId",
                table: "LocationManagers",
                column: "ManagersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationManagers");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Users",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
