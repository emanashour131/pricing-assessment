using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pricing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PriceLists",
                columns: new[] { "Id", "Currency", "MinQty", "PricePerUom", "Sku", "SupplierId", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "EUR", 100, 9.5m, "ABC123", 1, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "USD", 50, 10m, "ABC123", 2, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "EUR", 100, 9.5m, "ABC123", 3, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "EUR", 10, 5.25m, "XYZ777", 1, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "USD", 10, 5.40m, "XYZ777", 2, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "HazardClass", "Name", "Sku", "Uom" },
                values: new object[,]
                {
                    { 1, "None", "Product ABC", "ABC123", "pcs" },
                    { 2, "Flammable", "Product XYZ", "XYZ777", "pcs" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Active", "Country", "LeadTimeDays", "Name", "Preferred" },
                values: new object[,]
                {
                    { 2, true, "US", 3, "Supplier B", false },
                    { 3, true, "FR", 4, "Supplier C", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PriceLists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PriceLists",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PriceLists",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PriceLists",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PriceLists",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
