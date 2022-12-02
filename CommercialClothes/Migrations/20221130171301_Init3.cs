using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialClothes.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpiredDate",
                table: "Bank",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartedDate",
                table: "Bank",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "Bank");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "Bank");
        }
    }
}
