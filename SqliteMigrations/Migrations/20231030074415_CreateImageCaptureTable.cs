using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class CreateImageCaptureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageCaptureSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdateUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteStatus = table.Column<byte>(type: "INTEGER", nullable: false),
                    PageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PaperOrientation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCaptureSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageCaptureSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageCaptureLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SealType = table.Column<byte>(type: "INTEGER", nullable: false),
                    SubSealType = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageCaptureSettingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Left = table.Column<float>(type: "REAL", nullable: false),
                    Top = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCaptureLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageCaptureLocations_ImageCaptureSettings_ImageCaptureSettingId",
                        column: x => x.ImageCaptureSettingId,
                        principalTable: "ImageCaptureSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageCaptureLocations_ImageCaptureSettingId",
                table: "ImageCaptureLocations",
                column: "ImageCaptureSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageCaptureSettings_UserId",
                table: "ImageCaptureSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCaptureLocations");

            migrationBuilder.DropTable(
                name: "ImageCaptureSettings");
        }
    }
}
