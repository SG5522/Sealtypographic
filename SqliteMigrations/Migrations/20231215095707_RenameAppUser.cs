using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class RenameAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountantGroups_Users_CreateUserId",
                table: "AccountantGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountantGroups_Users_UpdateUserId",
                table: "AccountantGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_Users_CreateUserId",
                table: "Accountants");

            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_Users_UpdateUserId",
                table: "Accountants");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountantSignGroups_Users_CreateUserId",
                table: "AccountantSignGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountantSignGroups_Users_UpdateUserId",
                table: "AccountantSignGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Companys_Users_CreateUserId",
                table: "Companys");

            migrationBuilder.DropForeignKey(
                name: "FK_Companys_Users_UpdateUserId",
                table: "Companys");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_CreateUserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_UpdateUserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSealGroups_Users_CreateUserId",
                table: "CustomerSealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSealGroups_Users_UpdateUserId",
                table: "CustomerSealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageRangeSettings_Users_CreateUserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageRangeSettings_Users_UpdateUserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Letterheads_Users_CreateUserId",
                table: "Letterheads");

            migrationBuilder.DropForeignKey(
                name: "FK_Letterheads_Users_UpdateUserId",
                table: "Letterheads");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarterYears_Users_CreateUserId",
                table: "QuarterYears");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarterYears_Users_UpdateUserId",
                table: "QuarterYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Users_CreateUserId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Users_UpdateUserId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealGroups_Users_CreateUserId",
                table: "TemporarySealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealGroups_Users_UpdateUserId",
                table: "TemporarySealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicPDFs_Users_CreateUserId",
                table: "TypographicPDFs");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicPDFs_Users_UpdateUserId",
                table: "TypographicPDFs");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicResources_Users_CreateUserId",
                table: "TypographicResources");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicResources_Users_UpdateUserId",
                table: "TypographicResources");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_Users_CreateUserId",
                table: "UploadFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_Users_UpdateUserId",
                table: "UploadFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companys_CompanyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreateUserId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UpdateUserId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "ApplicationUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UpdateUserId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_UpdateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CreateUserId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_CreateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CompanyId",
                table: "ApplicationUsers",
                newName: "IX_ApplicationUsers_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_ApplicationUsers_CreateUserId",
                table: "AccountantGroups",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_ApplicationUsers_UpdateUserId",
                table: "AccountantGroups",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_ApplicationUsers_CreateUserId",
                table: "Accountants",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_ApplicationUsers_UpdateUserId",
                table: "Accountants",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantSignGroups_ApplicationUsers_CreateUserId",
                table: "AccountantSignGroups",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantSignGroups_ApplicationUsers_UpdateUserId",
                table: "AccountantSignGroups",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ApplicationUsers_CreateUserId",
                table: "ApplicationUsers",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ApplicationUsers_UpdateUserId",
                table: "ApplicationUsers",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Companys_CompanyId",
                table: "ApplicationUsers",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_ApplicationUsers_CreateUserId",
                table: "Companys",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_ApplicationUsers_UpdateUserId",
                table: "Companys",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_ApplicationUsers_CreateUserId",
                table: "Customers",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_ApplicationUsers_UpdateUserId",
                table: "Customers",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSealGroups_ApplicationUsers_CreateUserId",
                table: "CustomerSealGroups",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSealGroups_ApplicationUsers_UpdateUserId",
                table: "CustomerSealGroups",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRangeSettings_ApplicationUsers_CreateUserId",
                table: "ImageRangeSettings",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRangeSettings_ApplicationUsers_UpdateUserId",
                table: "ImageRangeSettings",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Letterheads_ApplicationUsers_CreateUserId",
                table: "Letterheads",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Letterheads_ApplicationUsers_UpdateUserId",
                table: "Letterheads",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterYears_ApplicationUsers_CreateUserId",
                table: "QuarterYears",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterYears_ApplicationUsers_UpdateUserId",
                table: "QuarterYears",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_ApplicationUsers_CreateUserId",
                table: "Templates",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_ApplicationUsers_UpdateUserId",
                table: "Templates",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealGroups_ApplicationUsers_CreateUserId",
                table: "TemporarySealGroups",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealGroups_ApplicationUsers_UpdateUserId",
                table: "TemporarySealGroups",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicPDFs_ApplicationUsers_CreateUserId",
                table: "TypographicPDFs",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicPDFs_ApplicationUsers_UpdateUserId",
                table: "TypographicPDFs",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicResources_ApplicationUsers_CreateUserId",
                table: "TypographicResources",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicResources_ApplicationUsers_UpdateUserId",
                table: "TypographicResources",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_ApplicationUsers_CreateUserId",
                table: "UploadFiles",
                column: "CreateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_ApplicationUsers_UpdateUserId",
                table: "UploadFiles",
                column: "UpdateUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountantGroups_ApplicationUsers_CreateUserId",
                table: "AccountantGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountantGroups_ApplicationUsers_UpdateUserId",
                table: "AccountantGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_ApplicationUsers_CreateUserId",
                table: "Accountants");

            migrationBuilder.DropForeignKey(
                name: "FK_Accountants_ApplicationUsers_UpdateUserId",
                table: "Accountants");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountantSignGroups_ApplicationUsers_CreateUserId",
                table: "AccountantSignGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountantSignGroups_ApplicationUsers_UpdateUserId",
                table: "AccountantSignGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ApplicationUsers_CreateUserId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ApplicationUsers_UpdateUserId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Companys_CompanyId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Companys_ApplicationUsers_CreateUserId",
                table: "Companys");

            migrationBuilder.DropForeignKey(
                name: "FK_Companys_ApplicationUsers_UpdateUserId",
                table: "Companys");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_ApplicationUsers_CreateUserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_ApplicationUsers_UpdateUserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSealGroups_ApplicationUsers_CreateUserId",
                table: "CustomerSealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSealGroups_ApplicationUsers_UpdateUserId",
                table: "CustomerSealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageRangeSettings_ApplicationUsers_CreateUserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageRangeSettings_ApplicationUsers_UpdateUserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Letterheads_ApplicationUsers_CreateUserId",
                table: "Letterheads");

            migrationBuilder.DropForeignKey(
                name: "FK_Letterheads_ApplicationUsers_UpdateUserId",
                table: "Letterheads");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarterYears_ApplicationUsers_CreateUserId",
                table: "QuarterYears");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarterYears_ApplicationUsers_UpdateUserId",
                table: "QuarterYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_ApplicationUsers_CreateUserId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_ApplicationUsers_UpdateUserId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealGroups_ApplicationUsers_CreateUserId",
                table: "TemporarySealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporarySealGroups_ApplicationUsers_UpdateUserId",
                table: "TemporarySealGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicPDFs_ApplicationUsers_CreateUserId",
                table: "TypographicPDFs");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicPDFs_ApplicationUsers_UpdateUserId",
                table: "TypographicPDFs");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicResources_ApplicationUsers_CreateUserId",
                table: "TypographicResources");

            migrationBuilder.DropForeignKey(
                name: "FK_TypographicResources_ApplicationUsers_UpdateUserId",
                table: "TypographicResources");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_ApplicationUsers_CreateUserId",
                table: "UploadFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_ApplicationUsers_UpdateUserId",
                table: "UploadFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_UpdateUserId",
                table: "Users",
                newName: "IX_Users_UpdateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_CreateUserId",
                table: "Users",
                newName: "IX_Users_CreateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsers_CompanyId",
                table: "Users",
                newName: "IX_Users_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_Users_CreateUserId",
                table: "AccountantGroups",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantGroups_Users_UpdateUserId",
                table: "AccountantGroups",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_Users_CreateUserId",
                table: "Accountants",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accountants_Users_UpdateUserId",
                table: "Accountants",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantSignGroups_Users_CreateUserId",
                table: "AccountantSignGroups",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountantSignGroups_Users_UpdateUserId",
                table: "AccountantSignGroups",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_Users_CreateUserId",
                table: "Companys",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_Users_UpdateUserId",
                table: "Companys",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_CreateUserId",
                table: "Customers",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_UpdateUserId",
                table: "Customers",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSealGroups_Users_CreateUserId",
                table: "CustomerSealGroups",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSealGroups_Users_UpdateUserId",
                table: "CustomerSealGroups",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRangeSettings_Users_CreateUserId",
                table: "ImageRangeSettings",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRangeSettings_Users_UpdateUserId",
                table: "ImageRangeSettings",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Letterheads_Users_CreateUserId",
                table: "Letterheads",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Letterheads_Users_UpdateUserId",
                table: "Letterheads",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterYears_Users_CreateUserId",
                table: "QuarterYears",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterYears_Users_UpdateUserId",
                table: "QuarterYears",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Users_CreateUserId",
                table: "Templates",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Users_UpdateUserId",
                table: "Templates",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealGroups_Users_CreateUserId",
                table: "TemporarySealGroups",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporarySealGroups_Users_UpdateUserId",
                table: "TemporarySealGroups",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicPDFs_Users_CreateUserId",
                table: "TypographicPDFs",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicPDFs_Users_UpdateUserId",
                table: "TypographicPDFs",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicResources_Users_CreateUserId",
                table: "TypographicResources",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypographicResources_Users_UpdateUserId",
                table: "TypographicResources",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_Users_CreateUserId",
                table: "UploadFiles",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_Users_UpdateUserId",
                table: "UploadFiles",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companys_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreateUserId",
                table: "Users",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UpdateUserId",
                table: "Users",
                column: "UpdateUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
