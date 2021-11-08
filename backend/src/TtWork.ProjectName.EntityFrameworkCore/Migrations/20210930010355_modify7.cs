using Microsoft.EntityFrameworkCore.Migrations;

namespace TtWork.ProjectName.Migrations
{
    public partial class modify7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PickupWay",
                table: "Activity_LuckDrawPrizes",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupWay",
                table: "Activity_LuckDrawPrizes");
        }
    }
}
