using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEditImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypographicEditImageFullPath",
                table: "TypographicResources");

            migrationBuilder.AddColumn<string>(
                name: "EditImageFullPath",
                table: "TypographicResourceLocations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditImageFullPath",
                table: "TypographicResourceLocations");

            migrationBuilder.AddColumn<string>(
                name: "TypographicEditImageFullPath",
                table: "TypographicResources",
                type: "TEXT",
                nullable: true);
        }
    }
}
