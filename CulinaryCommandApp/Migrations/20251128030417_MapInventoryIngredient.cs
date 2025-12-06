using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class MapInventoryIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_MeasurementUnits_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<int>(
                name: "MeasurementUnitUnitId",
                table: "RecipeIngredients",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Ingredients",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ingredients",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StockQuantity",
                table: "Ingredients",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            // Make UnitId nullable initially to allow safe migration of existing rows
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Ingredients",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Abbreviation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConversionFactor = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Seed common units so we can map existing DefaultUnit string values safely
            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Abbreviation", "Name", "ConversionFactor" },
                values: new object[,]
                {
                    { 1, "g", "gram", 1m },
                    { 2, "kg", "kilogram", 1000m },
                    { 3, "L", "liter", 1m },
                    { 4, "each", "each", 1m }
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    StockChange = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_MeasurementUnitUnitId",
                table: "RecipeIngredients",
                column: "MeasurementUnitUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UnitId",
                table: "Ingredients",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_IngredientId",
                table: "InventoryTransactions",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_UnitId",
                table: "InventoryTransactions",
                column: "UnitId");

            // Map existing string DefaultUnit values into the new Units table.
            // DefaultUnit column still exists at this point in the schema, so perform an UPDATE using SQL.
            // If DefaultUnit values don't exactly match abbreviations used above, adjust mapping accordingly.
            migrationBuilder.Sql(@"
                UPDATE Ingredients
                SET UnitId = (
                    SELECT Id FROM Units WHERE Units.Abbreviation = Ingredients.DefaultUnit
                )
                WHERE UnitId IS NULL AND DefaultUnit IS NOT NULL;
            ");

            // After migrating data, drop the DefaultUnit column
            migrationBuilder.DropColumn(
                name: "DefaultUnit",
                table: "Ingredients");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                // use Restrict to avoid accidental cascade deletes
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_MeasurementUnits_MeasurementUnitUnitId",
                table: "RecipeIngredients",
                column: "MeasurementUnitUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Units_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Units_UnitId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_MeasurementUnits_MeasurementUnitUnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Units_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4 });

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_MeasurementUnitUnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_UnitId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitUnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Ingredients");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingredients",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Ingredients",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            // add DefaultUnit back in Down()
            migrationBuilder.AddColumn<string>(
                name: "DefaultUnit",
                table: "Ingredients",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_MeasurementUnits_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
