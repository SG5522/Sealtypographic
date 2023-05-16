using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
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
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    BAN = table.Column<string>(type: "TEXT", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accountants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    AccountantGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Accountants_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountantSignTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PaperOrientation = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageViewFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignTemplates_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Customers_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSealTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StackMode = table.Column<int>(type: "INTEGER", nullable: false),
                    StackShift = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PaperOrientation = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageViewFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealTemplates_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterheadImageTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PaperOrientation = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageViewFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterheadImageTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterheadImageTemplates_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Letterheads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Letterheads_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadFiles_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Account = table.Column<string>(type: "TEXT", nullable: false),
                    Pwaosrsd = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
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
                name: "AccountantSignTemplateLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigType = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountantSignTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignTemplateLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignTemplateLocations_AccountantSignTemplates_AccountantSignTemplateId",
                        column: x => x.AccountantSignTemplateId,
                        principalTable: "AccountantSignTemplates",
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
                name: "TemporarySealQuarterJournals",
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
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporarySealQuarterJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporarySealQuarterJournals_Customers_CustomerId",
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
                    ReviewUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<sbyte>(type: "INTEGER", nullable: false)
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
                name: "CustomerSealTemplateLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigType = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerSealTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealTemplateLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealTemplateLocations_CustomerSealTemplates_CustomerSealTemplateId",
                        column: x => x.CustomerSealTemplateId,
                        principalTable: "CustomerSealTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterheadImageTemplateLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LetterheadImageTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterheadImageTemplateLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterheadImageTemplateLocations_LetterheadImageTemplates_LetterheadImageTemplateId",
                        column: x => x.LetterheadImageTemplateId,
                        principalTable: "LetterheadImageTemplates",
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
                name: "AccountantSignJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfigType = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountantSignGroupJournalId = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_AccountantSignJournals_AccountantSignGroupJournals_AccountantSignGroupJournalId",
                        column: x => x.AccountantSignGroupJournalId,
                        principalTable: "AccountantSignGroupJournals",
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
                name: "TemporarySealJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    TemporarySealQuarterJournalId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_TemporarySealJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporarySealJournals_TemporarySealQuarterJournals_TemporarySealQuarterJournalId",
                        column: x => x.TemporarySealQuarterJournalId,
                        principalTable: "TemporarySealQuarterJournals",
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
                    BlankCheck = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeleteCheck = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAccountantCertificate = table.Column<bool>(type: "INTEGER", nullable: false),
                    TypographicPDFId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "TypographicSealLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerSealJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountantSignJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    LetterheadImageJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    TemporarySealJournalId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypographicSealLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypographicSealLocations_AccountantSignJournals_AccountantSignJournalId",
                        column: x => x.AccountantSignJournalId,
                        principalTable: "AccountantSignJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicSealLocations_CustomerSealJournals_CustomerSealJournalId",
                        column: x => x.CustomerSealJournalId,
                        principalTable: "CustomerSealJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicSealLocations_LetterheadImageJournals_LetterheadImageJournalId",
                        column: x => x.LetterheadImageJournalId,
                        principalTable: "LetterheadImageJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicSealLocations_TemporarySealJournals_TemporarySealJournalId",
                        column: x => x.TemporarySealJournalId,
                        principalTable: "TemporarySealJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicSealLocations_TypographicPages_TypographicPageId",
                        column: x => x.TypographicPageId,
                        principalTable: "TypographicPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_AccountantGroupId",
                table: "Accountants",
                column: "AccountantGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_CompanyId",
                table: "Accountants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroupJournals_AccountantId",
                table: "AccountantSignGroupJournals",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignJournals_AccountantSignGroupJournalId",
                table: "AccountantSignJournals",
                column: "AccountantSignGroupJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignTemplateLocations_AccountantSignTemplateId",
                table: "AccountantSignTemplateLocations",
                column: "AccountantSignTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignTemplates_CompanyId",
                table: "AccountantSignTemplates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealJournals_CustomerSealQuarterJournalId",
                table: "CustomerSealJournals",
                column: "CustomerSealQuarterJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealQuarterJournals_CustomerId",
                table: "CustomerSealQuarterJournals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealTemplateLocations_CustomerSealTemplateId",
                table: "CustomerSealTemplateLocations",
                column: "CustomerSealTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealTemplates_CompanyId",
                table: "CustomerSealTemplates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageJournals_LetterheadId",
                table: "LetterheadImageJournals",
                column: "LetterheadId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageTemplateLocations_LetterheadImageTemplateId",
                table: "LetterheadImageTemplateLocations",
                column: "LetterheadImageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterheadImageTemplates_CompanyId",
                table: "LetterheadImageTemplates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_CompanyId",
                table: "Letterheads",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealJournals_TemporarySealQuarterJournalId",
                table: "TemporarySealJournals",
                column: "TemporarySealQuarterJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealQuarterJournals_CustomerId",
                table: "TemporarySealQuarterJournals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPages_TypographicPDFId",
                table: "TypographicPages",
                column: "TypographicPDFId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_CustomerId",
                table: "TypographicPDFs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicSealLocations_AccountantSignJournalId",
                table: "TypographicSealLocations",
                column: "AccountantSignJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicSealLocations_CustomerSealJournalId",
                table: "TypographicSealLocations",
                column: "CustomerSealJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicSealLocations_LetterheadImageJournalId",
                table: "TypographicSealLocations",
                column: "LetterheadImageJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicSealLocations_TemporarySealJournalId",
                table: "TypographicSealLocations",
                column: "TemporarySealJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicSealLocations_TypographicPageId",
                table: "TypographicSealLocations",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_CompanyId",
                table: "UploadFiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountantSignTemplateLocations");

            migrationBuilder.DropTable(
                name: "CustomerSealTemplateLocations");

            migrationBuilder.DropTable(
                name: "LetterheadImageTemplateLocations");

            migrationBuilder.DropTable(
                name: "TypographicSealLocations");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AccountantSignTemplates");

            migrationBuilder.DropTable(
                name: "CustomerSealTemplates");

            migrationBuilder.DropTable(
                name: "LetterheadImageTemplates");

            migrationBuilder.DropTable(
                name: "AccountantSignJournals");

            migrationBuilder.DropTable(
                name: "CustomerSealJournals");

            migrationBuilder.DropTable(
                name: "LetterheadImageJournals");

            migrationBuilder.DropTable(
                name: "TemporarySealJournals");

            migrationBuilder.DropTable(
                name: "TypographicPages");

            migrationBuilder.DropTable(
                name: "AccountantSignGroupJournals");

            migrationBuilder.DropTable(
                name: "CustomerSealQuarterJournals");

            migrationBuilder.DropTable(
                name: "Letterheads");

            migrationBuilder.DropTable(
                name: "TemporarySealQuarterJournals");

            migrationBuilder.DropTable(
                name: "TypographicPDFs");

            migrationBuilder.DropTable(
                name: "Accountants");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AccountantGroups");

            migrationBuilder.DropTable(
                name: "Companys");
        }
    }
}
