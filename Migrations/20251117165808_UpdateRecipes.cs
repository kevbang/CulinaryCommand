using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Locations_LocationId",
                table: "Recipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Ingredient_IngredientId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_MeasurementUnit_UnitId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Recipe_RecipeId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeStep_Recipe_RecipeId",
                table: "RecipeStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeStep",
                table: "RecipeStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredient",
                table: "RecipeIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeasurementUnit",
                table: "MeasurementUnit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.RenameTable(
                name: "RecipeStep",
                newName: "RecipeSteps");

            migrationBuilder.RenameTable(
                name: "RecipeIngredient",
                newName: "RecipeIngredients");

            migrationBuilder.RenameTable(
                name: "Recipe",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "MeasurementUnit",
                newName: "MeasurementUnits");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeStep_RecipeId",
                table: "RecipeSteps",
                newName: "IX_RecipeSteps_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredient_UnitId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_LocationId",
                table: "Recipes",
                newName: "IX_Recipes_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeSteps",
                table: "RecipeSteps",
                column: "StepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                column: "RecipeIngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeasurementUnits",
                table: "MeasurementUnits",
                column: "UnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_MeasurementUnits_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Locations_LocationId",
                table: "Recipes",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeSteps_Recipes_RecipeId",
                table: "RecipeSteps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_MeasurementUnits_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Locations_LocationId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeSteps_Recipes_RecipeId",
                table: "RecipeSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeSteps",
                table: "RecipeSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeasurementUnits",
                table: "MeasurementUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "RecipeSteps",
                newName: "RecipeStep");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipe");

            migrationBuilder.RenameTable(
                name: "RecipeIngredients",
                newName: "RecipeIngredient");

            migrationBuilder.RenameTable(
                name: "MeasurementUnits",
                newName: "MeasurementUnit");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeSteps_RecipeId",
                table: "RecipeStep",
                newName: "IX_RecipeStep_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_LocationId",
                table: "Recipe",
                newName: "IX_Recipe_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_UnitId",
                table: "RecipeIngredient",
                newName: "IX_RecipeIngredient_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredient",
                newName: "IX_RecipeIngredient_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredient",
                newName: "IX_RecipeIngredient_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeStep",
                table: "RecipeStep",
                column: "StepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe",
                column: "RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredient",
                table: "RecipeIngredient",
                column: "RecipeIngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeasurementUnit",
                table: "MeasurementUnit",
                column: "UnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Locations_LocationId",
                table: "Recipe",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Ingredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_MeasurementUnit_UnitId",
                table: "RecipeIngredient",
                column: "UnitId",
                principalTable: "MeasurementUnit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Recipe_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeStep_Recipe_RecipeId",
                table: "RecipeStep",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
