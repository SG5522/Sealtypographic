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
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Quarters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GregorianYear = table.Column<string>(type: "TEXT", nullable: false),
                    TaiwanYear = table.Column<string>(type: "TEXT", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountantGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountantGroupNumber = table.Column<string>(type: "TEXT", nullable: false),
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
                    table.PrimaryKey("PK_AccountantGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantGroups_Companys_CompanyId",
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
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_Customers_Companys_CompanyId",
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
                    Status = table.Column<sbyte>(type: "INTEGER", nullable: false),
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
                name: "Templates",
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
                    StackMode = table.Column<int>(type: "INTEGER", nullable: true),
                    StackShift = table.Column<int>(type: "INTEGER", nullable: true),
                    ImageViewFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
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
                name: "CustomerSealGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_CustomerSealGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSealGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSealGroups_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemporarySealGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporarySealGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporarySealGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporarySealGroups_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
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
                    QuarterId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    UploadFileId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_TypographicPDFs_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
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
                name: "AccountantSignGroups",
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
                    table.PrimaryKey("PK_AccountantSignGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountantSignGroups_Accountants_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "Accountants",
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
                name: "TypographicResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    SealType = table.Column<byte>(type: "INTEGER", nullable: false),
                    SubSealType = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerSealGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccountantSignGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    LetterheadId = table.Column<int>(type: "INTEGER", nullable: true),
                    TemporarySealGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    UploadFileId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    ImageFullPath = table.Column<string>(type: "TEXT", nullable: false),
                    ThumbnailFullPath = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "TypographicResourceLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                name: "IX_Accountants_AccountantGroupId",
                table: "Accountants",
                column: "AccountantGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_CompanyId",
                table: "Accountants",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroups_AccountantId",
                table: "AccountantSignGroups",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_CustomerId",
                table: "CustomerSealGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_QuarterId",
                table: "CustomerSealGroups",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_CompanyId",
                table: "Letterheads",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateLocations_TemplateId",
                table: "TemplateLocations",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CompanyId",
                table: "Templates",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_CustomerId",
                table: "TemporarySealGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_QuarterId",
                table: "TemporarySealGroups",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPages_TypographicPDFId",
                table: "TypographicPages",
                column: "TypographicPDFId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_CustomerId",
                table: "TypographicPDFs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_QuarterId",
                table: "TypographicPDFs",
                column: "QuarterId");

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
                name: "IX_TypographicResources_UploadFileId",
                table: "TypographicResources",
                column: "UploadFileId");

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
                name: "TemplateLocations");

            migrationBuilder.DropTable(
                name: "TypographicResourceLocations");

            migrationBuilder.DropTable(
                name: "Users");

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
                name: "Quarters");

            migrationBuilder.DropTable(
                name: "AccountantGroups");

            migrationBuilder.DropTable(
                name: "Companys");
        }
    }
}
