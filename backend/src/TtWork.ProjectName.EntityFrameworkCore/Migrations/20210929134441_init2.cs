using Microsoft.EntityFrameworkCore.Migrations;

namespace TtWork.ProjectName.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Audit_AuditNodes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Audit_AuditNodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Audit_AuditNodes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Audit_AuditNodes");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Audit_AuditNodes");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Audit_AuditNodes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
