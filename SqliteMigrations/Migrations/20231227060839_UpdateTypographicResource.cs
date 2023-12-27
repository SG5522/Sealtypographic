using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypographicResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Angle",
                table: "TypographicResourceLocations");

            migrationBuilder.DropColumn(
                name: "IsInpaint",
                table: "TypographicResourceLocations");

            migrationBuilder.DropColumn(
                name: "SealDyeing",
                table: "TypographicResourceLocations");

            migrationBuilder.AddColumn<string>(
                name: "EditPdfImageFullPath",
                table: "TypographicResources",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageProcessingFullPath",
                table: "TypographicResources",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditPdfImageFullPath",
                table: "TypographicResources");

            migrationBuilder.DropColumn(
                name: "ImageProcessingFullPath",
                table: "TypographicResources");

            migrationBuilder.AddColumn<float>(
                name: "Angle",
                table: "TypographicResourceLocations",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInpaint",
                table: "TypographicResourceLocations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SealDyeing",
                table: "TypographicResourceLocations",
                type: "INTEGER",
                nullable: true);
        }
    }
}
