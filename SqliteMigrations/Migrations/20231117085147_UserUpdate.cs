using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageRangeSettings_Users_UserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companys_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ImageRangeSettings_UserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ImageRangeSettings");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "UploadFiles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "UploadFiles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "TypographicResources",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "TypographicResources",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "TypographicPDFs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "TypographicPDFs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "TemporarySealGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "TemporarySealGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Templates",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Templates",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "QuarterYears",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "QuarterYears",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Letterheads",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Letterheads",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "ImageRangeSettings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "ImageRangeSettings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "CustomerSealGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "CustomerSealGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Customers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Customers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Companys",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Companys",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "AccountantSignGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "AccountantSignGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Accountants",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Accountants",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "AccountantGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "AccountantGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_CreateUserId",
                table: "UploadFiles",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_UpdateUserId",
                table: "UploadFiles",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_CreateUserId",
                table: "TypographicResources",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicResources_UpdateUserId",
                table: "TypographicResources",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_CreateUserId",
                table: "TypographicPDFs",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TypographicPDFs_UpdateUserId",
                table: "TypographicPDFs",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_CreateUserId",
                table: "TemporarySealGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporarySealGroups_UpdateUserId",
                table: "TemporarySealGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CreateUserId",
                table: "Templates",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_UpdateUserId",
                table: "Templates",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterYears_CreateUserId",
                table: "QuarterYears",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterYears_UpdateUserId",
                table: "QuarterYears",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_CreateUserId",
                table: "Letterheads",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Letterheads_UpdateUserId",
                table: "Letterheads",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeSettings_CreateUserId",
                table: "ImageRangeSettings",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeSettings_UpdateUserId",
                table: "ImageRangeSettings",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_CreateUserId",
                table: "CustomerSealGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSealGroups_UpdateUserId",
                table: "CustomerSealGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreateUserId",
                table: "Customers",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UpdateUserId",
                table: "Customers",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_CreateUserId",
                table: "Companys",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_UpdateUserId",
                table: "Companys",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroups_CreateUserId",
                table: "AccountantSignGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantSignGroups_UpdateUserId",
                table: "AccountantSignGroups",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_CreateUserId",
                table: "Accountants",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_UpdateUserId",
                table: "Accountants",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantGroups_CreateUserId",
                table: "AccountantGroups",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountantGroups_UpdateUserId",
                table: "AccountantGroups",
                column: "UpdateUserId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_CreateUserId",
                table: "UploadFiles");

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_UpdateUserId",
                table: "UploadFiles");

            migrationBuilder.DropIndex(
                name: "IX_TypographicResources_CreateUserId",
                table: "TypographicResources");

            migrationBuilder.DropIndex(
                name: "IX_TypographicResources_UpdateUserId",
                table: "TypographicResources");

            migrationBuilder.DropIndex(
                name: "IX_TypographicPDFs_CreateUserId",
                table: "TypographicPDFs");

            migrationBuilder.DropIndex(
                name: "IX_TypographicPDFs_UpdateUserId",
                table: "TypographicPDFs");

            migrationBuilder.DropIndex(
                name: "IX_TemporarySealGroups_CreateUserId",
                table: "TemporarySealGroups");

            migrationBuilder.DropIndex(
                name: "IX_TemporarySealGroups_UpdateUserId",
                table: "TemporarySealGroups");

            migrationBuilder.DropIndex(
                name: "IX_Templates_CreateUserId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_UpdateUserId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_QuarterYears_CreateUserId",
                table: "QuarterYears");

            migrationBuilder.DropIndex(
                name: "IX_QuarterYears_UpdateUserId",
                table: "QuarterYears");

            migrationBuilder.DropIndex(
                name: "IX_Letterheads_CreateUserId",
                table: "Letterheads");

            migrationBuilder.DropIndex(
                name: "IX_Letterheads_UpdateUserId",
                table: "Letterheads");

            migrationBuilder.DropIndex(
                name: "IX_ImageRangeSettings_CreateUserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropIndex(
                name: "IX_ImageRangeSettings_UpdateUserId",
                table: "ImageRangeSettings");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSealGroups_CreateUserId",
                table: "CustomerSealGroups");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSealGroups_UpdateUserId",
                table: "CustomerSealGroups");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreateUserId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UpdateUserId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Companys_CreateUserId",
                table: "Companys");

            migrationBuilder.DropIndex(
                name: "IX_Companys_UpdateUserId",
                table: "Companys");

            migrationBuilder.DropIndex(
                name: "IX_AccountantSignGroups_CreateUserId",
                table: "AccountantSignGroups");

            migrationBuilder.DropIndex(
                name: "IX_AccountantSignGroups_UpdateUserId",
                table: "AccountantSignGroups");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_CreateUserId",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_UpdateUserId",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_AccountantGroups_CreateUserId",
                table: "AccountantGroups");

            migrationBuilder.DropIndex(
                name: "IX_AccountantGroups_UpdateUserId",
                table: "AccountantGroups");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "UploadFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "UploadFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "TypographicResources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "TypographicResources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "TypographicPDFs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "TypographicPDFs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "TemporarySealGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "TemporarySealGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Templates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Templates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "QuarterYears",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "QuarterYears",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Letterheads",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Letterheads",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "ImageRangeSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "ImageRangeSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ImageRangeSettings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "CustomerSealGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "CustomerSealGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Companys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Companys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "AccountantSignGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "AccountantSignGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "Accountants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "Accountants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdateUserId",
                table: "AccountantGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreateUserId",
                table: "AccountantGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageRangeSettings_UserId",
                table: "ImageRangeSettings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRangeSettings_Users_UserId",
                table: "ImageRangeSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companys_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
