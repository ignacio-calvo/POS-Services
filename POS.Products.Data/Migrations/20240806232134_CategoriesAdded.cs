using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POS.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComboMeal",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MenuCategoryDisplayOrder",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MenuItemImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShouldPrintLabel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShouldPrintNutritionalLabel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShouldPrintTicket",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "MenuItemTypeCode",
                table: "Products",
                newName: "ProductTypeCode");

            migrationBuilder.RenameColumn(
                name: "MenuCategoryId",
                table: "Products",
                newName: "ProductImageUrl");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ImageUrl", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Variety of pizzas", "pizzas.jpg", null, null, "Pizzas" },
                    { 2, null, null, "Side dishes like cheese sticks", "sides.jpg", null, null, "Sides" },
                    { 3, null, null, "Drinks", "beverages.jpg", null, null, "Beverages" }
                });

            migrationBuilder.UpdateData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DefaultSize", "DisplayOrder", "Name", "Price", "StatusCode" },
                values: new object[] { true, (short)1, "Small", 8.99m, (short)1 });

            migrationBuilder.InsertData(
                table: "ProductSize",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DefaultSize", "DisplayOrder", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "PriceByWeight", "ProductId", "StatusCode", "TareWeight" },
                values: new object[,]
                {
                    { 2, null, null, false, (short)2, null, null, "Medium", 10.99m, false, 1, (short)1, 0f },
                    { 3, null, null, false, (short)3, null, null, "Large", 12.99m, false, 1, (short)1, 0f }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DisplayOrder", "Name", "ProductImageUrl", "ProductTypeCode", "StatusCode" },
                values: new object[] { "Classic pizza with tomato, mozzarella, and basil", 1, "Margherita Pizza", "margherita.jpg", (short)1, (short)1 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "IsPizza", "IsPrepared", "IsSpecialtyPizza", "IsTaxable", "KitchenDescription", "LabelDescription", "LastModifiedBy", "LastModifiedDate", "Name", "OrderDescription", "ProductImageUrl", "ProductTypeCode", "ReceiptDescription", "ShouldPromptForSize", "StatusCode" },
                values: new object[,]
                {
                    { 2, null, null, "Pizza with pepperoni and cheese", 2, null, null, null, null, null, null, null, null, "Pepperoni Pizza", null, "pepperoni.jpg", (short)1, null, false, (short)1 },
                    { 3, null, null, "Baked cheese sticks", 3, null, null, null, null, null, null, null, null, "Cheese Sticks", null, "cheesesticks.jpg", (short)2, null, false, (short)1 },
                    { 4, null, null, "Soft drink", 4, null, null, null, null, null, null, null, null, "Coca-Cola", null, "cocacola.jpg", (short)3, null, false, (short)1 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "ProductSize",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DefaultSize", "DisplayOrder", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "PriceByWeight", "ProductId", "StatusCode", "TareWeight" },
                values: new object[,]
                {
                    { 4, null, null, true, (short)1, null, null, "Small", 9.99m, false, 2, (short)1, 0f },
                    { 5, null, null, false, (short)2, null, null, "Medium", 11.99m, false, 2, (short)1, 0f },
                    { 6, null, null, false, (short)3, null, null, "Large", 13.99m, false, 2, (short)1, 0f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "ProductCategory",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DeleteData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "ProductTypeCode",
                table: "Products",
                newName: "MenuItemTypeCode");

            migrationBuilder.RenameColumn(
                name: "ProductImageUrl",
                table: "Products",
                newName: "MenuCategoryId");

            migrationBuilder.AddColumn<bool>(
                name: "IsComboMeal",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MenuCategoryDisplayOrder",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MenuItemImage",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldPrintLabel",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldPrintNutritionalLabel",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldPrintTicket",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductSize",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DefaultSize", "DisplayOrder", "Name", "Price", "StatusCode" },
                values: new object[] { false, (short)0, "Large", 18.99m, (short)0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DisplayOrder", "IsComboMeal", "MenuCategoryDisplayOrder", "MenuCategoryId", "MenuItemImage", "MenuItemTypeCode", "Name", "ShouldPrintLabel", "ShouldPrintNutritionalLabel", "ShouldPrintTicket", "StatusCode" },
                values: new object[] { null, 0, null, null, null, null, (short)0, "Mozarella Pizza", null, null, null, (short)0 });
        }
    }
}
