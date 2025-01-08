using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PO_Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_user_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_purchase_orders_purchaser_id",
                schema: "PO",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "purchaser_id",
                schema: "PO",
                table: "PurchaseOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "purchaser_id",
                schema: "PO",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_purchase_orders_purchaser_id",
                schema: "PO",
                table: "PurchaseOrders",
                column: "purchaser_id");
        }
    }
}
