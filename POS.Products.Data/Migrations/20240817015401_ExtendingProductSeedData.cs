using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POS.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendingProductSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DefaultSize",
                value: false);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DefaultSize",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DefaultSize",
                value: false);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DefaultSize",
                value: true);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "IsPizza", "IsPrepared", "IsSpecialtyPizza", "IsTaxable", "KitchenDescription", "LabelDescription", "LastModifiedBy", "LastModifiedDate", "Name", "OrderDescription", "ProductImageUrl", "ProductTypeCode", "ReceiptDescription", "ShouldPromptForSize", "StatusCode" },
                values: new object[,]
                {
                    { 8, null, null, "Pizza with ham and pineapple", 8, null, null, null, null, null, null, null, null, "Hawaiian Pizza", null, "hawaiian.png", (short)1, null, false, (short)1 },
                    { 9, null, null, "Pizza with various vegetables", 9, null, null, null, null, null, null, null, null, "Veggie Pizza", null, "veggie.png", (short)1, null, false, (short)1 },
                    { 10, null, null, "Pizza with BBQ chicken", 10, null, null, null, null, null, null, null, null, "BBQ Chicken Pizza", null, "bbqchicken.png", (short)1, null, false, (short)1 },
                    { 11, null, null, "Pizza with buffalo chicken", 11, null, null, null, null, null, null, null, null, "Buffalo Chicken Pizza", null, "buffalochicken.png", (short)1, null, false, (short)1 },
                    { 12, null, null, "Pizza with various meats", 12, null, null, null, null, null, null, null, null, "Meat Lovers Pizza", null, "meatlovers.png", (short)1, null, false, (short)1 },
                    { 13, null, null, "Pizza with various toppings", 13, null, null, null, null, null, null, null, null, "Supreme Pizza", null, "supreme.png", (short)1, null, false, (short)1 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 },
                    { 1, 13 }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DefaultSize", "DisplayOrder", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "PriceByWeight", "ProductId", "StatusCode", "TareWeight" },
                values: new object[,]
                {
                    { 13, null, null, false, (short)1, null, null, "Small", 9.99m, false, 8, (short)1, 0f },
                    { 14, null, null, true, (short)2, null, null, "Medium", 11.99m, false, 8, (short)1, 0f },
                    { 15, null, null, false, (short)3, null, null, "Large", 13.99m, false, 8, (short)1, 0f },
                    { 16, null, null, false, (short)1, null, null, "Small", 9.99m, false, 9, (short)1, 0f },
                    { 17, null, null, true, (short)2, null, null, "Medium", 11.99m, false, 9, (short)1, 0f },
                    { 18, null, null, false, (short)3, null, null, "Large", 13.99m, false, 9, (short)1, 0f },
                    { 19, null, null, false, (short)1, null, null, "Small", 9.99m, false, 10, (short)1, 0f },
                    { 20, null, null, true, (short)2, null, null, "Medium", 11.99m, false, 10, (short)1, 0f },
                    { 21, null, null, false, (short)3, null, null, "Large", 13.99m, false, 10, (short)1, 0f },
                    { 22, null, null, false, (short)1, null, null, "Small", 9.99m, false, 11, (short)1, 0f },
                    { 23, null, null, true, (short)2, null, null, "Medium", 11.99m, false, 11, (short)1, 0f },
                    { 24, null, null, false, (short)3, null, null, "Large", 13.99m, false, 11, (short)1, 0f },
                    { 25, null, null, false, (short)1, null, null, "Small", 9.99m, false, 12, (short)1, 0f },
                    { 26, null, null, true, (short)2, null, null, "Medium", 11.99m, false, 12, (short)1, 0f },
                    { 27, null, null, false, (short)3, null, null, "Large", 13.99m, false, 12, (short)1, 0f },
                    { 28, null, null, false, (short)1, null, null, "Small", 9.99m, false, 13, (short)1, 0f },
                    { 29, null, null, true, (short)2, null, null, "Medium", 11.99m, false, 13, (short)1, 0f },
                    { 30, null, null, false, (short)3, null, null, "Large", 13.99m, false, 13, (short)1, 0f }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 12 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 1, 13 });

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DefaultSize",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DefaultSize",
                value: false);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DefaultSize",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DefaultSize",
                value: false);
        }
    }
}
