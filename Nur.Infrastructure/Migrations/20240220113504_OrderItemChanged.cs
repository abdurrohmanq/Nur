using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nur.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_CartItem_CartItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CartItemId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CartItemId",
                table: "OrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CartItemId",
                table: "OrderItems",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_CartItem_CartItemId",
                table: "OrderItems",
                column: "CartItemId",
                principalTable: "CartItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
