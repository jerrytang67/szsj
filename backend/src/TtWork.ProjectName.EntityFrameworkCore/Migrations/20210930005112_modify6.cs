using Microsoft.EntityFrameworkCore.Migrations;

namespace TtWork.ProjectName.Migrations
{
    public partial class modify6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PickupWay",
                table: "Activity_UserPrizes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupWay",
                table: "Activity_UserPrizes");
        }
    }
}
