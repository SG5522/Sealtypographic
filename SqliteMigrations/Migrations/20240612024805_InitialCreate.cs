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
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accountants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupAccountants",
                columns: table => new
                {
                    AccountantId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountantGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAccountants", x => new { x.AccountantGroupId, x.AccountantId });
                    table.ForeignKey(
                        name: "FK_GroupAccountants_AccountantGroups_AccountantGroupId",
                        column: x => x.AccountantGroupId,
                        principalTable: "AccountantGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAccountants_Accountants_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "Accountants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountantSignGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ReviewUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<sbyte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountantSignGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignGroups_Accountants_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "Accountants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublicKeyBase64 = table.Column<string>(type: "TEXT", nullable: false),
                    PrivateKeyFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
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
                    Fax = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companys_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companys_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuarterYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GregorianYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<byte>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuarterYears_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuarterYears_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
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
                    Fax = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageRangeSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    PageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PaperOrientation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageRangeSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageRangeSettings_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageRangeSettings_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageRangeSettings_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Letterheads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<sbyte>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letterheads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Letterheads_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Letterheads_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Letterheads_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    PageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PaperOrientation = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StackMode = table.Column<int>(type: "INTEGER", nullable: true),
                    StackShift = table.Column<int>(type: "INTEGER", nullable: true),
                    ImageViewFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Templates_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Templates_Companys_CompanyId",
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
                    EncryptKey = table.Column<string>(type: "TEXT", nullable: false),
                    FileWorkStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadFiles_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UploadFiles_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UploadFiles_Companys_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSealGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuarterYearId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographyType = table.Column<byte>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ReviewUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewStatus = table.Column<sbyte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSealGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealGroups_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerSealGroups_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerSealGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSealGroups_QuarterYears_QuarterYearId",
                        column: x => x.QuarterYearId,
                        principalTable: "QuarterYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemporarySealGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuarterYearId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporarySealGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporarySealGroups_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemporarySealGroups_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemporarySealGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporarySealGroups_QuarterYears_QuarterYearId",
                        column: x => x.QuarterYearId,
                        principalTable: "QuarterYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageRangeLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SealType = table.Column<byte>(type: "INTEGER", nullable: false),
                    SubSealType = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageRangeSettingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageRangeLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageRangeLocations_ImageRangeSettings_ImageRangeSettingId",
                        column: x => x.ImageRangeSettingId,
                        principalTable: "ImageRangeSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SealType = table.Column<byte>(type: "INTEGER", nullable: false),
                    SubSealType = table.Column<int>(type: "INTEGER", nullable: false),
                    TemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateLocations_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
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
                    TypographyType = table.Column<byte>(type: "INTEGER", nullable: false),
                    QuarterYearId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    UploadFileId = table.Column<int>(type: "INTEGER", nullable: false),
                    PdfEditStep = table.Column<byte>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
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
                        name: "FK_TypographicPDFs_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicPDFs_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicPDFs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicPDFs_QuarterYears_QuarterYearId",
                        column: x => x.QuarterYearId,
                        principalTable: "QuarterYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicPDFs_UploadFiles_UploadFileId",
                        column: x => x.UploadFileId,
                        principalTable: "UploadFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypographicResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    SealType = table.Column<byte>(type: "INTEGER", nullable: false),
                    SubSealType = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageProcessingFullPath = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerSealGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccountantSignGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    LetterheadId = table.Column<int>(type: "INTEGER", nullable: true),
                    TemporarySealGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    UploadFileId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ImageFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ImageEncryptKey = table.Column<string>(type: "TEXT", nullable: true),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: true),
                    ThumbnailEncryptKey = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypographicResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypographicResources_AccountantSignGroups_AccountantSignGroupId",
                        column: x => x.AccountantSignGroupId,
                        principalTable: "AccountantSignGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicResources_ApplicationUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicResources_ApplicationUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicResources_CustomerSealGroups_CustomerSealGroupId",
                        column: x => x.CustomerSealGroupId,
                        principalTable: "CustomerSealGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicResources_Letterheads_LetterheadId",
                        column: x => x.LetterheadId,
                        principalTable: "Letterheads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicResources_TemporarySealGroups_TemporarySealGroupId",
                        column: x => x.TemporarySealGroupId,
                        principalTable: "TemporarySealGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypographicResources_UploadFiles_UploadFileId",
                        column: x => x.UploadFileId,
                        principalTable: "UploadFiles",
                        principalColumn: "Id");
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
                    AccountantCertificateFileId = table.Column<int>(type: "INTEGER", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_TypographicPages_UploadFiles_AccountantCertificateFileId",
                        column: x => x.AccountantCertificateFileId,
                        principalTable: "UploadFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TypographicResourceLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EditImageFullPath = table.Column<string>(type: "TEXT", nullable: true),
                    TypographicResourceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TypographicPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypographicResourceLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypographicResourceLocations_TypographicPages_TypographicPageId",
                        column: x => x.TypographicPageId,
                        principalTable: "TypographicPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypographicResourceLocations_TypographicResources_TypographicResourceId",
                        column: x => x.TypographicResourceId,
                        principalTable: "TypographicResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountantGroups_CompanyId",
                table: "AccountantGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantGroups_CreateUserId",
                table: "AccountantGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantGroups_UpdateUserId",
                table: "AccountantGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_CompanyId",
                table: "Accountants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_CreateUserId",
                table: "Accountants",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_UpdateUserId",
                table: "Accountants",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroups_AccountantId",
                table: "AccountantSignGroups",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroups_CreateUserId",
                table: "AccountantSignGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroups_UpdateUserId",
                table: "AccountantSignGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CompanyId",
                table: "ApplicationUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_CreateUserId",
                table: "ApplicationUsers",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_UpdateUserId",
                table: "ApplicationUsers",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_CreateUserId",
                table: "Companys",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_UpdateUserId",
                table: "Companys",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreateUserId",
                table: "Customers",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UpdateUserId",
                table: "Customers",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_CreateUserId",
                table: "CustomerSealGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_CustomerId",
                table: "CustomerSealGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_QuarterYearId",
                table: "CustomerSealGroups",
                column: "QuarterYearId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_UpdateUserId",
                table: "CustomerSealGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccountants_AccountantId",
                table: "GroupAccountants",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeLocations_ImageRangeSettingId",
                table: "ImageRangeLocations",
                column: "ImageRangeSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeSettings_CompanyId",
                table: "ImageRangeSettings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeSettings_CreateUserId",
                table: "ImageRangeSettings",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeSettings_UpdateUserId",
                table: "ImageRangeSettings",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_CompanyId",
                table: "Letterheads",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_CreateUserId",
                table: "Letterheads",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_UpdateUserId",
                table: "Letterheads",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterYears_CreateUserId",
                table: "QuarterYears",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterYears_UpdateUserId",
                table: "QuarterYears",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLocations_TemplateId",
                table: "TemplateLocations",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CompanyId",
                table: "Templates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CreateUserId",
                table: "Templates",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_UpdateUserId",
                table: "Templates",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_CreateUserId",
                table: "TemporarySealGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_CustomerId",
                table: "TemporarySealGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_QuarterYearId",
                table: "TemporarySealGroups",
                column: "QuarterYearId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_UpdateUserId",
                table: "TemporarySealGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPages_AccountantCertificateFileId",
                table: "TypographicPages",
                column: "AccountantCertificateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPages_TypographicPDFId",
                table: "TypographicPages",
                column: "TypographicPDFId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_CreateUserId",
                table: "TypographicPDFs",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_CustomerId",
                table: "TypographicPDFs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_QuarterYearId",
                table: "TypographicPDFs",
                column: "QuarterYearId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_UpdateUserId",
                table: "TypographicPDFs",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_UploadFileId",
                table: "TypographicPDFs",
                column: "UploadFileId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResourceLocations_TypographicPageId",
                table: "TypographicResourceLocations",
                column: "TypographicPageId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResourceLocations_TypographicResourceId",
                table: "TypographicResourceLocations",
                column: "TypographicResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_AccountantSignGroupId",
                table: "TypographicResources",
                column: "AccountantSignGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_CreateUserId",
                table: "TypographicResources",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_CustomerSealGroupId",
                table: "TypographicResources",
                column: "CustomerSealGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_LetterheadId",
                table: "TypographicResources",
                column: "LetterheadId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_TemporarySealGroupId",
                table: "TypographicResources",
                column: "TemporarySealGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_UpdateUserId",
                table: "TypographicResources",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_UploadFileId",
                table: "TypographicResources",
                column: "UploadFileId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_CompanyId",
                table: "UploadFiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_CreateUserId",
                table: "UploadFiles",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_UpdateUserId",
                table: "UploadFiles",
                column: "UpdateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_ApplicationUsers_CreateUserId",
                table: "AccountantGroups",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_ApplicationUsers_UpdateUserId",
                table: "AccountantGroups",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_Companys_CompanyId",
                table: "AccountantGroups",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_ApplicationUsers_CreateUserId",
                table: "Accountants",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_ApplicationUsers_UpdateUserId",
                table: "Accountants",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_Companys_CompanyId",
                table: "Accountants",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantSignGroups_ApplicationUsers_CreateUserId",
                table: "AccountantSignGroups",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantSignGroups_ApplicationUsers_UpdateUserId",
                table: "AccountantSignGroups",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Companys_CompanyId",
                table: "ApplicationUsers",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companys_ApplicationUsers_CreateUserId",
                table: "Companys");

            migrationBuilder.DropForeignKey(
                name: "FK_Companys_ApplicationUsers_UpdateUserId",
                table: "Companys");

            migrationBuilder.DropTable(
                name: "GroupAccountants");

            migrationBuilder.DropTable(
                name: "ImageRangeLocations");

            migrationBuilder.DropTable(
                name: "TemplateLocations");

            migrationBuilder.DropTable(
                name: "TypographicResourceLocations");

            migrationBuilder.DropTable(
                name: "AccountantGroups");

            migrationBuilder.DropTable(
                name: "ImageRangeSettings");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "TypographicPages");

            migrationBuilder.DropTable(
                name: "TypographicResources");

            migrationBuilder.DropTable(
                name: "TypographicPDFs");

            migrationBuilder.DropTable(
                name: "AccountantSignGroups");

            migrationBuilder.DropTable(
                name: "CustomerSealGroups");

            migrationBuilder.DropTable(
                name: "Letterheads");

            migrationBuilder.DropTable(
                name: "TemporarySealGroups");

            migrationBuilder.DropTable(
                name: "UploadFiles");

            migrationBuilder.DropTable(
                name: "Accountants");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "QuarterYears");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Companys");
        }
    }
}
