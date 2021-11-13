using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoRep.Data.Migrations
{
    public partial class dbnamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_User_WorkerId",
                table: "Work");

            migrationBuilder.DropForeignKey(
                name: "FK_Work_WorkType_workTypeId",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Work_WorkerId",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Work_workTypeId",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "workTypeId",
                table: "Work");

            migrationBuilder.AddColumn<int>(
                name: "WorkType",
                table: "Work",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Worker",
                table: "Work",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkType",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "Worker",
                table: "Work");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workTypeId",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Work_WorkerId",
                table: "Work",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Work_workTypeId",
                table: "Work",
                column: "workTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Work_User_WorkerId",
                table: "Work",
                column: "WorkerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_WorkType_workTypeId",
                table: "Work",
                column: "workTypeId",
                principalTable: "WorkType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
