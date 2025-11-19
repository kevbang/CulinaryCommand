using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class MakeMarginEdgeKeyNullableAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MarginEdgeKey",
                table: "Locations",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(124)",
                oldMaxLength: 124)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "MarginEdgeKey",
                keyValue: null,
                column: "MarginEdgeKey",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MarginEdgeKey",
                table: "Locations",
                type: "varchar(124)",
                maxLength: 124,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
