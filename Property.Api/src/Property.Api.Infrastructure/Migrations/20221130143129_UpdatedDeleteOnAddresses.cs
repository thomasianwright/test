using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeleteOnAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Address_TradingAddressId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Address_PropertyAddressId",
                table: "Property");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Address_TradingAddressId",
                table: "Company",
                column: "TradingAddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Address_PropertyAddressId",
                table: "Property",
                column: "PropertyAddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Address_TradingAddressId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Address_PropertyAddressId",
                table: "Property");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Address_TradingAddressId",
                table: "Company",
                column: "TradingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Address_PropertyAddressId",
                table: "Property",
                column: "PropertyAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
