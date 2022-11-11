using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialClothes.Migrations
{
    public partial class Add_DescriptionShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Shops",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Shops");
        }
    }
}
