using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyAndUserLocationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationUser_Locations_LocationsId",
                table: "LocationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationUser_Users_UsersId",
                table: "LocationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationUser",
                table: "LocationUser");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "LocationUser",
                newName: "UserLocations");

            migrationBuilder.RenameIndex(
                name: "IX_LocationUser_UsersId",
                table: "UserLocations",
                newName: "IX_UserLocations_UsersId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Location",
                keyValue: null,
                column: "Location",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Users",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MarginEdgeKey",
                table: "Locations",
                type: "varchar(124)",
                maxLength: 124,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLocations",
                table: "UserLocations",
                columns: new[] { "LocationsId", "UsersId" });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LLCName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaxId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CompanyId",
                table: "Locations",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Companies_CompanyId",
                table: "Locations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocations_Locations_LocationsId",
                table: "UserLocations",
                column: "LocationsId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocations_Users_UsersId",
                table: "UserLocations",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Companies_CompanyId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocations_Locations_LocationsId",
                table: "UserLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocations_Users_UsersId",
                table: "UserLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CompanyId",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLocations",
                table: "UserLocations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "MarginEdgeKey",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "UserLocations",
                newName: "LocationUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserLocations_UsersId",
                table: "LocationUser",
                newName: "IX_LocationUser_UsersId");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationUser",
                table: "LocationUser",
                columns: new[] { "LocationsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LocationUser_Locations_LocationsId",
                table: "LocationUser",
                column: "LocationsId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationUser_Users_UsersId",
                table: "LocationUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
