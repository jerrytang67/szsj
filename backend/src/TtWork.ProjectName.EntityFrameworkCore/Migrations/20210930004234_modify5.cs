using Microsoft.EntityFrameworkCore.Migrations;

namespace TtWork.ProjectName.Migrations
{
    public partial class modify5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_CityName",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_CountyName",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_DetailInfo",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_NationalCode",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_TelNumber",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_UserName",
                table: "Activity_UserPrizes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PickupWay",
                table: "Activity_LuckDraws",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_CityName",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "Address_CountyName",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "Address_DetailInfo",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "Address_NationalCode",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "Address_TelNumber",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "Address_UserName",
                table: "Activity_UserPrizes");

            migrationBuilder.DropColumn(
                name: "PickupWay",
                table: "Activity_LuckDraws");
        }
    }
}
