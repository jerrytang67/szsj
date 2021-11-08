using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TtWork.ProjectName.Migrations
{
    public partial class modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserLuckTimeId",
                table: "QA_UserQuestionLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LuckDrawId",
                table: "QA_QAPlans",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Activity_UserLuckTimes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Host = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    HostId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LuckDrawId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShareFrom = table.Column<long>(type: "bigint", nullable: true),
                    LuckDrawTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserPrizeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_UserLuckTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_UserLuckTimes_Activity_UserPrizes_UserPrizeId",
                        column: x => x.UserPrizeId,
                        principalTable: "Activity_UserPrizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_VoteItems_State_VotePlanId",
                table: "Activity_VoteItems",
                columns: new[] { "State", "VotePlanId" });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserLuckTimes_Status",
                table: "Activity_UserLuckTimes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserLuckTimes_UserPrizeId",
                table: "Activity_UserLuckTimes",
                column: "UserPrizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity_UserLuckTimes");

            migrationBuilder.DropIndex(
                name: "IX_Activity_VoteItems_State_VotePlanId",
                table: "Activity_VoteItems");

            migrationBuilder.DropColumn(
                name: "UserLuckTimeId",
                table: "QA_UserQuestionLogs");

            migrationBuilder.DropColumn(
                name: "LuckDrawId",
                table: "QA_QAPlans");
        }
    }
}
