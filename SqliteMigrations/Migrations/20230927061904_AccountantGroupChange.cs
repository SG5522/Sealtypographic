using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AccountantGroupChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_AccountantGroups_AccountantGroupId",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_AccountantGroupId",
                table: "Accountants");

            migrationBuilder.DropColumn(
                name: "AccountantGroupId",
                table: "Accountants");

            migrationBuilder.RenameColumn(
                name: "AccountantGroupNumber",
                table: "AccountantGroups",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "KeycloakUserId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccountants_AccountantId",
                table: "GroupAccountants",
                column: "AccountantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupAccountants");

            migrationBuilder.DropColumn(
                name: "KeycloakUserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "AccountantGroups",
                newName: "AccountantGroupNumber");

            migrationBuilder.AddColumn<int>(
                name: "AccountantGroupId",
                table: "Accountants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_AccountantGroupId",
                table: "Accountants",
                column: "AccountantGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_AccountantGroups_AccountantGroupId",
                table: "Accountants",
                column: "AccountantGroupId",
                principalTable: "AccountantGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
