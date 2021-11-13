using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoRep.Data.Migrations
{
    public partial class arrayremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkType_Work_WorkId",
                table: "WorkType");

            migrationBuilder.DropIndex(
                name: "IX_WorkType_WorkId",
                table: "WorkType");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "WorkType");

            migrationBuilder.AddColumn<int>(
                name: "workTypeId",
                table: "Work",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "WorkId",
                table: "WorkType",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkType_WorkId",
                table: "WorkType",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkType_Work_WorkId",
                table: "WorkType",
                column: "WorkId",
                principalTable: "Work",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
