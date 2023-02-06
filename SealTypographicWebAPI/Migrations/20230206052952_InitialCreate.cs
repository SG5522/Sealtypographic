using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SealTypographicWebAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountantGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantGroupNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", nullable: true),
                    President = table.Column<string>(type: "TEXT", nullable: true),
                    BAN = table.Column<string>(type: "TEXT", nullable: true),
                    StockCode = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    AddressCity = table.Column<string>(type: "TEXT", nullable: true),
                    AddressArea = table.Column<string>(type: "TEXT", nullable: true),
                    AddressStreet = table.Column<string>(type: "TEXT", nullable: true),
                    AddressLocate = table.Column<string>(type: "TEXT", nullable: true),
                    ContactName = table.Column<string>(type: "TEXT", nullable: true),
                    ContactTitle = table.Column<string>(type: "TEXT", nullable: true),
                    ContactTelephone = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Fax = table.Column<string>(type: "TEXT", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Letterheads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letterheads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UploadType = table.Column<byte>(type: "INTEGER", nullable: false),
                    OriginalFileName = table.Column<string>(type: "TEXT", nullable: false),
                    FullPath = table.Column<string>(type: "TEXT", nullable: false),
                    FileWorkStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", nullable: false),
                    Pwaosrsd = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accountants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    AccountantGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accountants_AccountantGroups_AccountantGroupId",
                        column: x => x.AccountantGroupId,
                        principalTable: "AccountantGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSealQuarterJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quarter = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ReviewUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<sbyte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealQuarterJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealQuarterJournals_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypographicPDFs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginFileName = table.Column<string>(type: "TEXT", nullable: false),
                    FullPath = table.Column<string>(type: "TEXT", nullable: false),
                    Quarter = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypographicPDFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypographicPDFs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterheadImageJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<sbyte>(type: "INTEGER", nullable: false),
                    LetterheadId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterheadImageJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterheadImageJournals_Letterheads_LetterheadId",
                        column: x => x.LetterheadId,
                        principalTable: "Letterheads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountantSignGroupJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ReviewUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<sbyte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignGroupJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignGroupJournals_Accountants_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "Accountants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSealJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigType = table.Column<int>(type: "INTEGER", nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerSealQuarterJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ImageFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealJournals_CustomerSealQuarterJournals_CustomerSealQuarterJournalId",
                        column: x => x.CustomerSealQuarterJournalId,
                        principalTable: "CustomerSealQuarterJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypographicPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PageNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPDFId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypographicPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypographicPages_TypographicPDFs_TypographicPDFId",
                        column: x => x.TypographicPDFId,
                        principalTable: "TypographicPDFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountantSignJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigType = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountantSignCreateDateJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ImageFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignJournals_AccountantSignGroupJournals_AccountantSignCreateDateJournalId",
                        column: x => x.AccountantSignCreateDateJournalId,
                        principalTable: "AccountantSignGroupJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSealLocaltions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerSealJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealLocaltions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealLocaltions_CustomerSealJournals_CustomerSealJournalId",
                        column: x => x.CustomerSealJournalId,
                        principalTable: "CustomerSealJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSealLocaltions_TypographicPages_TypographicPageId",
                        column: x => x.TypographicPageId,
                        principalTable: "TypographicPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterheadImageLocaltions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LetterheadImageJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterheadImageLocaltions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterheadImageLocaltions_LetterheadImageJournals_LetterheadImageJournalId",
                        column: x => x.LetterheadImageJournalId,
                        principalTable: "LetterheadImageJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetterheadImageLocaltions_TypographicPages_TypographicPageId",
                        column: x => x.TypographicPageId,
                        principalTable: "TypographicPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountantSignLocaltions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantSignJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignLocaltions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignLocaltions_AccountantSignJournals_AccountantSignJournalId",
                        column: x => x.AccountantSignJournalId,
                        principalTable: "AccountantSignJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountantSignLocaltions_TypographicPages_TypographicPageId",
                        column: x => x.TypographicPageId,
                        principalTable: "TypographicPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountantGroups",
                columns: new[] { "Id", "AccountantGroupNumber", "CreateDate", "CreateUserId", "DeleteStatus", "Name", "UpdateDate", "UpdateUserId" },
                values: new object[] { 1, "NO000", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)0, "預設群組", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_AccountantGroupId",
                table: "Accountants",
                column: "AccountantGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroupJournals_AccountantId",
                table: "AccountantSignGroupJournals",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignJournals_AccountantSignCreateDateJournalId",
                table: "AccountantSignJournals",
                column: "AccountantSignCreateDateJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignLocaltions_AccountantSignJournalId",
                table: "AccountantSignLocaltions",
                column: "AccountantSignJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignLocaltions_TypographicPageId",
                table: "AccountantSignLocaltions",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealJournals_CustomerSealQuarterJournalId",
                table: "CustomerSealJournals",
                column: "CustomerSealQuarterJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealLocaltions_CustomerSealJournalId",
                table: "CustomerSealLocaltions",
                column: "CustomerSealJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealLocaltions_TypographicPageId",
                table: "CustomerSealLocaltions",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealQuarterJournals_CustomerId",
                table: "CustomerSealQuarterJournals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageJournals_LetterheadId",
                table: "LetterheadImageJournals",
                column: "LetterheadId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageLocaltions_LetterheadImageJournalId",
                table: "LetterheadImageLocaltions",
                column: "LetterheadImageJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageLocaltions_TypographicPageId",
                table: "LetterheadImageLocaltions",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPages_TypographicPDFId",
                table: "TypographicPages",
                column: "TypographicPDFId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_CustomerId",
                table: "TypographicPDFs",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountantSignLocaltions");

            migrationBuilder.DropTable(
                name: "CustomerSealLocaltions");

            migrationBuilder.DropTable(
                name: "LetterheadImageLocaltions");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AccountantSignJournals");

            migrationBuilder.DropTable(
                name: "CustomerSealJournals");

            migrationBuilder.DropTable(
                name: "LetterheadImageJournals");

            migrationBuilder.DropTable(
                name: "TypographicPages");

            migrationBuilder.DropTable(
                name: "AccountantSignGroupJournals");

            migrationBuilder.DropTable(
                name: "CustomerSealQuarterJournals");

            migrationBuilder.DropTable(
                name: "Letterheads");

            migrationBuilder.DropTable(
                name: "TypographicPDFs");

            migrationBuilder.DropTable(
                name: "Accountants");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AccountantGroups");
        }
    }
}
