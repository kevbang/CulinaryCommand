using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Tasks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Tasks",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Par",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Tasks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IngredientId",
                table: "Tasks",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_LocationId",
                table: "Tasks",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_RecipeId",
                table: "Tasks",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Ingredients_IngredientId",
                table: "Tasks",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Locations_LocationId",
                table: "Tasks",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Recipes_RecipeId",
                table: "Tasks",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Ingredients_IngredientId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Locations_LocationId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Recipes_RecipeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_IngredientId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_LocationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_RecipeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Par",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Tasks");
        }
    }
}
