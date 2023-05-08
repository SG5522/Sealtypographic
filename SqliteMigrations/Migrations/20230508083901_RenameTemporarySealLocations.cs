using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class RenameTemporarySealLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSealTemplateLocations_CustomerSealTemplates_CustomerTemplateId",
                table: "CustomerSealTemplateLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LetterheadImageTemplateLocations_LetterheadImageTemplates_CustomerTemplateId",
                table: "LetterheadImageTemplateLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealLocation_TemporarySealJournals_TemporarySealJournalId",
                table: "TemporarySealLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealLocation_TypographicPages_TypographicPageId",
                table: "TemporarySealLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemporarySealLocation",
                table: "TemporarySealLocation");

            migrationBuilder.RenameTable(
                name: "TemporarySealLocation",
                newName: "TemporarySealLocations");

            migrationBuilder.RenameColumn(
                name: "CustomerTemplateId",
                table: "LetterheadImageTemplateLocations",
                newName: "LetterheadImageTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_LetterheadImageTemplateLocations_CustomerTemplateId",
                table: "LetterheadImageTemplateLocations",
                newName: "IX_LetterheadImageTemplateLocations_LetterheadImageTemplateId");

            migrationBuilder.RenameColumn(
                name: "CustomerTemplateId",
                table: "CustomerSealTemplateLocations",
                newName: "CustomerSealTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerSealTemplateLocations_CustomerTemplateId",
                table: "CustomerSealTemplateLocations",
                newName: "IX_CustomerSealTemplateLocations_CustomerSealTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_TemporarySealLocation_TypographicPageId",
                table: "TemporarySealLocations",
                newName: "IX_TemporarySealLocations_TypographicPageId");

            migrationBuilder.RenameIndex(
                name: "IX_TemporarySealLocation_TemporarySealJournalId",
                table: "TemporarySealLocations",
                newName: "IX_TemporarySealLocations_TemporarySealJournalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemporarySealLocations",
                table: "TemporarySealLocations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSealTemplateLocations_CustomerSealTemplates_CustomerSealTemplateId",
                table: "CustomerSealTemplateLocations",
                column: "CustomerSealTemplateId",
                principalTable: "CustomerSealTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LetterheadImageTemplateLocations_LetterheadImageTemplates_LetterheadImageTemplateId",
                table: "LetterheadImageTemplateLocations",
                column: "LetterheadImageTemplateId",
                principalTable: "LetterheadImageTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealLocations_TemporarySealJournals_TemporarySealJournalId",
                table: "TemporarySealLocations",
                column: "TemporarySealJournalId",
                principalTable: "TemporarySealJournals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealLocations_TypographicPages_TypographicPageId",
                table: "TemporarySealLocations",
                column: "TypographicPageId",
                principalTable: "TypographicPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSealTemplateLocations_CustomerSealTemplates_CustomerSealTemplateId",
                table: "CustomerSealTemplateLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_LetterheadImageTemplateLocations_LetterheadImageTemplates_LetterheadImageTemplateId",
                table: "LetterheadImageTemplateLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealLocations_TemporarySealJournals_TemporarySealJournalId",
                table: "TemporarySealLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealLocations_TypographicPages_TypographicPageId",
                table: "TemporarySealLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemporarySealLocations",
                table: "TemporarySealLocations");

            migrationBuilder.RenameTable(
                name: "TemporarySealLocations",
                newName: "TemporarySealLocation");

            migrationBuilder.RenameColumn(
                name: "LetterheadImageTemplateId",
                table: "LetterheadImageTemplateLocations",
                newName: "CustomerTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_LetterheadImageTemplateLocations_LetterheadImageTemplateId",
                table: "LetterheadImageTemplateLocations",
                newName: "IX_LetterheadImageTemplateLocations_CustomerTemplateId");

            migrationBuilder.RenameColumn(
                name: "CustomerSealTemplateId",
                table: "CustomerSealTemplateLocations",
                newName: "CustomerTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerSealTemplateLocations_CustomerSealTemplateId",
                table: "CustomerSealTemplateLocations",
                newName: "IX_CustomerSealTemplateLocations_CustomerTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_TemporarySealLocations_TypographicPageId",
                table: "TemporarySealLocation",
                newName: "IX_TemporarySealLocation_TypographicPageId");

            migrationBuilder.RenameIndex(
                name: "IX_TemporarySealLocations_TemporarySealJournalId",
                table: "TemporarySealLocation",
                newName: "IX_TemporarySealLocation_TemporarySealJournalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemporarySealLocation",
                table: "TemporarySealLocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSealTemplateLocations_CustomerSealTemplates_CustomerTemplateId",
                table: "CustomerSealTemplateLocations",
                column: "CustomerTemplateId",
                principalTable: "CustomerSealTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LetterheadImageTemplateLocations_LetterheadImageTemplates_CustomerTemplateId",
                table: "LetterheadImageTemplateLocations",
                column: "CustomerTemplateId",
                principalTable: "LetterheadImageTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealLocation_TemporarySealJournals_TemporarySealJournalId",
                table: "TemporarySealLocation",
                column: "TemporarySealJournalId",
                principalTable: "TemporarySealJournals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealLocation_TypographicPages_TypographicPageId",
                table: "TemporarySealLocation",
                column: "TypographicPageId",
                principalTable: "TypographicPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
