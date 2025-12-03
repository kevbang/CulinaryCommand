using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class AddPrepFieldsToWorkTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Tasks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Assigner",
                keyValue: null,
                column: "Assigner",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Assigner",
                table: "Tasks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Tasks",
                type: "int",
                nullable: true);

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
                name: "Par",
                table: "Tasks",
                type: "int",
                nullable: true);

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
                name: "FK_Tasks_Recipes_RecipeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_IngredientId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_RecipeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Par",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Tasks",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Assigner",
                table: "Tasks",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
