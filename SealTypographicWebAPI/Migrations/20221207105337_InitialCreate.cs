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
                    CustomerNumber = table.Column<string>(type: "TEXT", nullable: false),
                    BAN = table.Column<string>(type: "TEXT", nullable: false),
                    StockCode = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    Fax = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<int>(type: "INTEGER", nullable: false),
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
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letterheads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SealMappingConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SealType = table.Column<string>(type: "TEXT", nullable: false),
                    SubId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SealMappingConfigs", x => x.Id);
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
                    AccountantNumber = table.Column<string>(type: "TEXT", nullable: false),
                    AccountantGroupId = table.Column<string>(type: "TEXT", nullable: false),
                    AccountantGroupId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accountants_AccountantGroups_AccountantGroupId1",
                        column: x => x.AccountantGroupId1,
                        principalTable: "AccountantGroups",
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
                name: "CustomerSealJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    Quarter = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    SealMappingConfigId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealJournals_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSealJournals_SealMappingConfigs_SealMappingConfigId",
                        column: x => x.SealMappingConfigId,
                        principalTable: "SealMappingConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterheadImageJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    LetterheadId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    SealMappingConfigId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_LetterheadImageJournals_SealMappingConfigs_SealMappingConfigId",
                        column: x => x.SealMappingConfigId,
                        principalTable: "SealMappingConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountantSignJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    SealMappingConfigId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignJournals_Accountants_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "Accountants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountantSignJournals_SealMappingConfigs_SealMappingConfigId",
                        column: x => x.SealMappingConfigId,
                        principalTable: "SealMappingConfigs",
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
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "AccountantSingLocaltions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantSignJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSingLocaltions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSingLocaltions_AccountantSignJournals_AccountantSignJournalId",
                        column: x => x.AccountantSignJournalId,
                        principalTable: "AccountantSignJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountantSingLocaltions_TypographicPages_TypographicPageId",
                        column: x => x.TypographicPageId,
                        principalTable: "TypographicPages",
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

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_AccountantGroupId1",
                table: "Accountants",
                column: "AccountantGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignJournals_AccountantId",
                table: "AccountantSignJournals",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignJournals_SealMappingConfigId",
                table: "AccountantSignJournals",
                column: "SealMappingConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSingLocaltions_AccountantSignJournalId",
                table: "AccountantSingLocaltions",
                column: "AccountantSignJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSingLocaltions_TypographicPageId",
                table: "AccountantSingLocaltions",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealJournals_CustomerId",
                table: "CustomerSealJournals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealJournals_SealMappingConfigId",
                table: "CustomerSealJournals",
                column: "SealMappingConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealLocaltions_CustomerSealJournalId",
                table: "CustomerSealLocaltions",
                column: "CustomerSealJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealLocaltions_TypographicPageId",
                table: "CustomerSealLocaltions",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageJournals_LetterheadId",
                table: "LetterheadImageJournals",
                column: "LetterheadId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageJournals_SealMappingConfigId",
                table: "LetterheadImageJournals",
                column: "SealMappingConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageLocaltions_LetterheadImageJournalId",
                table: "LetterheadImageLocaltions",
                column: "LetterheadImageJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageLocaltions_TypographicPageId",
                table: "LetterheadImageLocaltions",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_SealMappingConfigs_SubId",
                table: "SealMappingConfigs",
                column: "SubId",
                unique: true);

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
                name: "AccountantSingLocaltions");

            migrationBuilder.DropTable(
                name: "CustomerSealLocaltions");

            migrationBuilder.DropTable(
                name: "LetterheadImageLocaltions");

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
                name: "Accountants");

            migrationBuilder.DropTable(
                name: "Letterheads");

            migrationBuilder.DropTable(
                name: "SealMappingConfigs");

            migrationBuilder.DropTable(
                name: "TypographicPDFs");

            migrationBuilder.DropTable(
                name: "AccountantGroups");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
