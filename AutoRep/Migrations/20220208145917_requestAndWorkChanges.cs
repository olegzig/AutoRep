using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoRep.Migrations
{
    public partial class requestAndWorkChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactData",
                table: "Request");

            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarNumber",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Work",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Work",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Request",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Request");

            migrationBuilder.AddColumn<string>(
                name: "ContactData",
                table: "Request",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
