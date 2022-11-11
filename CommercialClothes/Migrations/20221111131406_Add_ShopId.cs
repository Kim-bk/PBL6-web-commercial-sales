using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialClothes.Migrations
{
    public partial class Add_ShopId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "OrderDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDetails");
        }
    }
}
