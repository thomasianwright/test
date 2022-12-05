using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineThree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradingAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Address_TradingAddressId",
                        column: x => x.TradingAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyAddressId = table.Column<int>(type: "int", nullable: false),
                    PropertyCompanyId = table.Column<int>(type: "int", nullable: false),
                    PropertyRentalAgreementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Property_Address_PropertyAddressId",
                        column: x => x.PropertyAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Property_Company_PropertyCompanyId",
                        column: x => x.PropertyCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserAddressId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserCompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountUserOwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_User_AccountUserOwnerId",
                        column: x => x.AccountUserOwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountsId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountUser", x => new { x.AccountsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AccountUser_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountUser_Account_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentalAgreement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalAgreementPropertyId = table.Column<int>(type: "int", nullable: false),
                    RentalAgreementAccountId = table.Column<int>(type: "int", nullable: true),
                    RentalAgreementCompanyId = table.Column<int>(type: "int", nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalAgreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalAgreement_Account_RentalAgreementAccountId",
                        column: x => x.RentalAgreementAccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RentalAgreement_Company_RentalAgreementCompanyId",
                        column: x => x.RentalAgreementCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalAgreement_Property_RentalAgreementPropertyId",
                        column: x => x.RentalAgreementPropertyId,
                        principalTable: "Property",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RentalAgreement_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountUserOwnerId",
                table: "Account",
                column: "AccountUserOwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_AccountId",
                table: "AccountUser",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountUser_UserId",
                table: "AccountUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_TradingAddressId",
                table: "Company",
                column: "TradingAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyAddressId",
                table: "Property",
                column: "PropertyAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyCompanyId",
                table: "Property",
                column: "PropertyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreement_RentalAgreementAccountId",
                table: "RentalAgreement",
                column: "RentalAgreementAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreement_RentalAgreementCompanyId",
                table: "RentalAgreement",
                column: "RentalAgreementCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreement_RentalAgreementPropertyId",
                table: "RentalAgreement",
                column: "RentalAgreementPropertyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreement_UserId",
                table: "RentalAgreement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressId",
                table: "User",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CompanyId",
                table: "User",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountUser");

            migrationBuilder.DropTable(
                name: "RentalAgreement");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
