using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompKeyToAccountUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountUser",
                table: "AccountUser");

            migrationBuilder.DropIndex(
                name: "IX_AccountUser_UserId",
                table: "AccountUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountUser",
                table: "AccountUser",
                columns: new[] { "UserId", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_AccountsId",
                table: "AccountUser",
                column: "AccountsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountUser",
                table: "AccountUser");

            migrationBuilder.DropIndex(
                name: "IX_AccountUser_AccountsId",
                table: "AccountUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountUser",
                table: "AccountUser",
                columns: new[] { "AccountsId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_UserId",
                table: "AccountUser",
                column: "UserId");
        }
    }
}
