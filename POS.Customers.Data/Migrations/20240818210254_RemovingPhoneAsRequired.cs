using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingPhoneAsRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "john.doe@example.com", "John", "Doe", "3510000000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "ignaciocalvo@live.com.ar", "Ignacio", "Calvo", "3513335304" });
        }
    }
}
