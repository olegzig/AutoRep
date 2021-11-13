using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoRep.Data.Migrations
{
    public partial class arraytolist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_User_WorkerId",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Work_WorkerId",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Work");

            migrationBuilder.AddColumn<int>(
                name: "WorkId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_WorkId",
                table: "User",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Work_WorkId",
                table: "User",
                column: "WorkId",
                principalTable: "Work",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Work_WorkId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_WorkId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Work_WorkerId",
                table: "Work",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Work_User_WorkerId",
                table: "Work",
                column: "WorkerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
