using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialClothes.Migrations
{
    public partial class UpdateRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserGroups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");
        }
    }
}
