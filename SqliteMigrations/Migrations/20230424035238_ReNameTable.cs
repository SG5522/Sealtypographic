using System;
using DBEntities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ReNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignTemplateLocations_AccountantSignTemplateId",
                table: "AccountantSignTemplateLocations",
                column: "AccountantSignTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignTemplates_CompanyId",
                table: "AccountantSignTemplates",
                column: "CompanyId");

            migrationBuilder.Sql(
                "INSERT INTO AccountantSignTemplateLocations " +
                "SELECT * FROM AccountSignSealTemplateLocations");

            migrationBuilder.Sql(
                "INSERT INTO AccountantSignTemplates " +
                "SELECT * FROM AccountSignSealTemplates");

            migrationBuilder.DropTable(
                name: "AccountSignSealTemplateLocations");

            migrationBuilder.DropTable(
                name: "AccountSignSealTemplates");
        }        
    }
}
