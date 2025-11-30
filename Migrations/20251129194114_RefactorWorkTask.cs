using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class RefactorWorkTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Station",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Assigner",
                table: "Tasks",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "AssignerId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueAt",
                table: "Tasks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Tasks",
                type: "int",
                maxLength: 128,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Tasks",
                type: "int",
                maxLength: 128,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DueAt",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Tasks",
                newName: "Assigner");

            migrationBuilder.AddColumn<string>(
                name: "Station",
                table: "Tasks",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
