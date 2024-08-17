using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Products.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuCategoryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KitchenDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    MenuCategoryDisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsTaxable = table.Column<short>(type: "smallint", nullable: true),
                    IsPrepared = table.Column<bool>(type: "bit", nullable: true),
                    IsPizza = table.Column<bool>(type: "bit", nullable: true),
                    IsSpecialtyPizza = table.Column<bool>(type: "bit", nullable: true),
                    StatusCode = table.Column<short>(type: "smallint", nullable: false),
                    ShouldPrintLabel = table.Column<bool>(type: "bit", nullable: true),
                    ShouldPrintTicket = table.Column<bool>(type: "bit", nullable: true),
                    ShouldPrintNutritionalLabel = table.Column<bool>(type: "bit", nullable: true),
                    ShouldPromptForSize = table.Column<bool>(type: "bit", nullable: false),
                    IsComboMeal = table.Column<bool>(type: "bit", nullable: true),
                    MenuItemTypeCode = table.Column<short>(type: "smallint", nullable: false),
                    MenuItemImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductSize",
                columns: table => new
                {
                    SizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false),
                    StatusCode = table.Column<short>(type: "smallint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DefaultSize = table.Column<bool>(type: "bit", nullable: false),
                    PriceByWeight = table.Column<bool>(type: "bit", nullable: false),
                    TareWeight = table.Column<float>(type: "real", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSize", x => x.SizeId);
                    table.ForeignKey(
                        name: "FK_ProductSize_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_ProductId",
                table: "ProductSize",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
