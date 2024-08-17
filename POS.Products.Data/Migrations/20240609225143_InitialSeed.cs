using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "ProductSize",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductSize",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "IsComboMeal", "IsPizza", "IsPrepared", "IsSpecialtyPizza", "IsTaxable", "KitchenDescription", "LabelDescription", "LastModifiedBy", "LastModifiedDate", "MenuCategoryDisplayOrder", "MenuCategoryId", "MenuItemImage", "MenuItemTypeCode", "Name", "OrderDescription", "ReceiptDescription", "ShouldPrintLabel", "ShouldPrintNutritionalLabel", "ShouldPrintTicket", "ShouldPromptForSize", "StatusCode" },
                values: new object[] { 1, null, null, null, 0, null, null, null, null, null, null, null, null, null, null, null, null, (short)0, "Mozarella Pizza", null, null, null, null, null, false, (short)0 });

            migrationBuilder.InsertData(
                table: "ProductSize",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DefaultSize", "DisplayOrder", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "PriceByWeight", "ProductId", "StatusCode", "TareWeight" },
                values: new object[] { 1, null, null, false, (short)0, null, null, "Large", 18.99m, false, 1, (short)0, 0f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductSize",
                newName: "SizeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductSize",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "ProductSize",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "ProductSize",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
