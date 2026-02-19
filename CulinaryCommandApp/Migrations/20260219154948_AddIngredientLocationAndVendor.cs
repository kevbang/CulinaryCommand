using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryCommand.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientLocationAndVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Guard against partial runs: only add columns if they don't already exist
            migrationBuilder.Sql(@"
                SET @dbname = DATABASE();
                SET @tblname = 'Ingredients';

                SET @col1 = (SELECT COUNT(*) FROM information_schema.COLUMNS
                             WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = @tblname AND COLUMN_NAME = 'LocationId');
                SET @sql1 = IF(@col1 = 0,
                    'ALTER TABLE `Ingredients` ADD COLUMN `LocationId` int NOT NULL DEFAULT 0',
                    'SELECT 1');
                PREPARE stmt1 FROM @sql1; EXECUTE stmt1; DEALLOCATE PREPARE stmt1;

                SET @col2 = (SELECT COUNT(*) FROM information_schema.COLUMNS
                             WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = @tblname AND COLUMN_NAME = 'VendorId');
                SET @sql2 = IF(@col2 = 0,
                    'ALTER TABLE `Ingredients` ADD COLUMN `VendorId` int NULL',
                    'SELECT 1');
                PREPARE stmt2 FROM @sql2; EXECUTE stmt2; DEALLOCATE PREPARE stmt2;
            ");

            // Remove any orphaned rows that have no valid LocationId.
            // Must first remove referencing PurchaseOrderLines (and their parent PurchaseOrders) to
            // avoid the FK constraint from PurchaseOrderLines -> Ingredients.
            migrationBuilder.Sql(@"
                -- Nullify or delete PurchaseOrderLines that reference orphaned Ingredients
                DELETE pol FROM `PurchaseOrderLines` pol
                INNER JOIN `Ingredients` i ON pol.`IngredientId` = i.`IngredientId`
                WHERE i.`LocationId` = 0 OR i.`LocationId` NOT IN (SELECT `Id` FROM `Locations`);

                -- Now safe to delete orphaned Ingredients
                DELETE FROM `Ingredients`
                WHERE `LocationId` = 0 OR `LocationId` NOT IN (SELECT `Id` FROM `Locations`);
            ");

            // Guard indexes
            migrationBuilder.Sql(@"
                SET @dbname = DATABASE();

                SET @idx1 = (SELECT COUNT(*) FROM information_schema.STATISTICS
                             WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'Ingredients' AND INDEX_NAME = 'IX_Ingredients_LocationId');
                SET @sql3 = IF(@idx1 = 0,
                    'CREATE INDEX `IX_Ingredients_LocationId` ON `Ingredients` (`LocationId`)',
                    'SELECT 1');
                PREPARE stmt3 FROM @sql3; EXECUTE stmt3; DEALLOCATE PREPARE stmt3;

                SET @idx2 = (SELECT COUNT(*) FROM information_schema.STATISTICS
                             WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'Ingredients' AND INDEX_NAME = 'IX_Ingredients_VendorId');
                SET @sql4 = IF(@idx2 = 0,
                    'CREATE INDEX `IX_Ingredients_VendorId` ON `Ingredients` (`VendorId`)',
                    'SELECT 1');
                PREPARE stmt4 FROM @sql4; EXECUTE stmt4; DEALLOCATE PREPARE stmt4;
            ");

            // Guard FK: LocationId
            migrationBuilder.Sql(@"
                SET @dbname = DATABASE();

                SET @fk1 = (SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
                            WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'Ingredients'
                            AND CONSTRAINT_NAME = 'FK_Ingredients_Locations_LocationId');
                SET @sql5 = IF(@fk1 = 0,
                    'ALTER TABLE `Ingredients` ADD CONSTRAINT `FK_Ingredients_Locations_LocationId` FOREIGN KEY (`LocationId`) REFERENCES `Locations` (`Id`) ON DELETE CASCADE',
                    'SELECT 1');
                PREPARE stmt5 FROM @sql5; EXECUTE stmt5; DEALLOCATE PREPARE stmt5;

                SET @fk2 = (SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
                            WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'Ingredients'
                            AND CONSTRAINT_NAME = 'FK_Ingredients_Vendors_VendorId');
                SET @sql6 = IF(@fk2 = 0,
                    'ALTER TABLE `Ingredients` ADD CONSTRAINT `FK_Ingredients_Vendors_VendorId` FOREIGN KEY (`VendorId`) REFERENCES `Vendors` (`Id`) ON DELETE SET NULL',
                    'SELECT 1');
                PREPARE stmt6 FROM @sql6; EXECUTE stmt6; DEALLOCATE PREPARE stmt6;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Locations_LocationId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Vendors_VendorId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_LocationId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_VendorId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Ingredients");
        }
    }
}
