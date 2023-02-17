using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SealTypographicWebAPI.Migrations
{
    public partial class RenameColumnWithAccountantSignJournal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountantSignCreateDateJournalId",
                table: "AccountantSignJournals",
                newName: "AccountantSignGroupJournalId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountantSignJournals_AccountantSignCreateDateJournalId",
                table: "AccountantSignJournals",
                newName: "IX_AccountantSignJournals_AccountantSignGroupJournalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
        }
    }
}
