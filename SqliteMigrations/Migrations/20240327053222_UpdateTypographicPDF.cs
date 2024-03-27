using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypographicPDF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PdfEditStep",
                table: "TypographicPDFs",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfEditStep",
                table: "TypographicPDFs");
        }
    }
}
