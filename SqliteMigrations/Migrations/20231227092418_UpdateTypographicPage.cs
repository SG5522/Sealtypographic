using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypographicPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypographicPages_UploadFiles_UploadFileId",
                table: "TypographicPages");

            migrationBuilder.RenameColumn(
                name: "EditPdfImageFullPath",
                table: "TypographicResources",
                newName: "TypographicEditImageFullPath");

            migrationBuilder.RenameColumn(
                name: "UploadFileId",
                table: "TypographicPages",
                newName: "AccountantCertificateFileId");

            migrationBuilder.RenameIndex(
                name: "IX_TypographicPages_UploadFileId",
                table: "TypographicPages",
                newName: "IX_TypographicPages_AccountantCertificateFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicPages_UploadFiles_AccountantCertificateFileId",
                table: "TypographicPages",
                column: "AccountantCertificateFileId",
                principalTable: "UploadFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypographicPages_UploadFiles_AccountantCertificateFileId",
                table: "TypographicPages");

            migrationBuilder.RenameColumn(
                name: "TypographicEditImageFullPath",
                table: "TypographicResources",
                newName: "EditPdfImageFullPath");

            migrationBuilder.RenameColumn(
                name: "AccountantCertificateFileId",
                table: "TypographicPages",
                newName: "UploadFileId");

            migrationBuilder.RenameIndex(
                name: "IX_TypographicPages_AccountantCertificateFileId",
                table: "TypographicPages",
                newName: "IX_TypographicPages_UploadFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicPages_UploadFiles_UploadFileId",
                table: "TypographicPages",
                column: "UploadFileId",
                principalTable: "UploadFiles",
                principalColumn: "Id");
        }
    }
}
