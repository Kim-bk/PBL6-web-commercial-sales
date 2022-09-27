using Microsoft.EntityFrameworkCore.Migrations;

namespace CommercialClothes.Migrations
{
    public partial class UpdateNameOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Ordereds_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordereds_Accounts_AccountId",
                table: "Ordereds");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordereds_Payments_PaymentId",
                table: "Ordereds");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordereds_Statuses_StatusId",
                table: "Ordereds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ordereds",
                table: "Ordereds");

            migrationBuilder.RenameTable(
                name: "Ordereds",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Ordereds_StatusId",
                table: "Orders",
                newName: "IX_Orders_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Ordereds_PaymentId",
                table: "Orders",
                newName: "IX_Orders_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Ordereds_AccountId",
                table: "Orders",
                newName: "IX_Orders_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Statuses_StatusId",
                table: "Orders",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_AccountId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Statuses_StatusId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Ordereds");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_StatusId",
                table: "Ordereds",
                newName: "IX_Ordereds_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PaymentId",
                table: "Ordereds",
                newName: "IX_Ordereds_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AccountId",
                table: "Ordereds",
                newName: "IX_Ordereds_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ordereds",
                table: "Ordereds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Ordereds_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Ordereds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordereds_Accounts_AccountId",
                table: "Ordereds",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordereds_Payments_PaymentId",
                table: "Ordereds",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordereds_Statuses_StatusId",
                table: "Ordereds",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
