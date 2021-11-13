using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoRep.Data.Migrations
{
    public partial class fieldremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_WorkType_workTypeId",
                table: "Work");

            migrationBuilder.DropIndex(
                name: "IX_Work_workTypeId",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "workTypeId",
                table: "Work");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "workTypeId",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Work_workTypeId",
                table: "Work",
                column: "workTypeId");

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
