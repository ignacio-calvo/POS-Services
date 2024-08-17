using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POS.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "pizza-icon.png");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "sides-icon.png");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "beverages-icon.png");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ImageUrl", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 4, null, null, "Variety of chicken wings", "chickenwings-icon.png", null, null, "Chicken Wings" },
                    { 5, null, null, "Different types of pasta", "pasta-icon.png", null, null, "Pasta" },
                    { 6, null, null, "Fresh salads", "salad-icon.png", null, null, "Salad" }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DefaultSize", "DisplayOrder", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "PriceByWeight", "ProductId", "StatusCode", "TareWeight" },
                values: new object[,]
                {
                    { 7, null, null, true, (short)1, null, null, "Regular", 4.99m, false, 3, (short)1, 0f },
                    { 8, null, null, true, (short)1, null, null, "Can", 1.99m, false, 4, (short)1, 0f }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductImageUrl",
                value: "margherita.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductImageUrl",
                value: "pepperoni.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductImageUrl",
                value: "cheesesticks.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductImageUrl",
                value: "cocacola.png");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "IsPizza", "IsPrepared", "IsSpecialtyPizza", "IsTaxable", "KitchenDescription", "LabelDescription", "LastModifiedBy", "LastModifiedDate", "Name", "OrderDescription", "ProductImageUrl", "ProductTypeCode", "ReceiptDescription", "ShouldPromptForSize", "StatusCode" },
                values: new object[,]
                {
                    { 5, null, null, "Chicken wings with BBQ sauce", 5, null, null, null, null, null, null, null, null, "BBQ Chicken Wings", null, "bbqwings.png", (short)4, null, false, (short)1 },
                    { 6, null, null, "Spaghetti with meat sauce", 6, null, null, null, null, null, null, null, null, "Spaghetti Bolognese", null, "spaghetti.png", (short)5, null, false, (short)1 },
                    { 7, null, null, "Salad with romaine lettuce, croutons, and Caesar dressing", 7, null, null, null, null, null, null, null, null, "Caesar Salad", null, "caesarsalad.png", (short)6, null, false, (short)1 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 4, 5 },
                    { 5, 6 },
                    { 6, 7 }
                });

            migrationBuilder.InsertData(
                table: "ProductSizes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DefaultSize", "DisplayOrder", "LastModifiedBy", "LastModifiedDate", "Name", "Price", "PriceByWeight", "ProductId", "StatusCode", "TareWeight" },
                values: new object[,]
                {
                    { 9, null, null, true, (short)1, null, null, "6 Pieces", 6.99m, false, 5, (short)1, 0f },
                    { 10, null, null, false, (short)2, null, null, "12 Pieces", 12.99m, false, 5, (short)1, 0f },
                    { 11, null, null, true, (short)1, null, null, "Regular", 9.99m, false, 6, (short)1, 0f },
                    { 12, null, null, true, (short)1, null, null, "Regular", 7.99m, false, 7, (short)1, 0f }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { 6, 7 });

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductSizes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "pizzas.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "sides.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "beverages.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductImageUrl",
                value: "margherita.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductImageUrl",
                value: "pepperoni.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductImageUrl",
                value: "cheesesticks.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductImageUrl",
                value: "cocacola.jpg");
        }
    }
}
