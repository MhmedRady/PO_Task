using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PO_Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Purchase_Order_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PO");

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "PO",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    purchaser_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    po_number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_amount_currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                schema: "PO",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    good_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    serial_number = table.Column<int>(type: "int", nullable: false),
                    purchase_order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    price_currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_purchase_orders_purchase_order_id",
                        column: x => x.purchase_order_id,
                        principalSchema: "PO",
                        principalTable: "PurchaseOrders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_items_purchase_order_id",
                schema: "PO",
                table: "OrderItems",
                column: "purchase_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_purchase_orders_po_number",
                schema: "PO",
                table: "PurchaseOrders",
                column: "po_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchase_orders_purchaser_id",
                schema: "PO",
                table: "PurchaseOrders",
                column: "purchaser_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "PO");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "PO");
        }
    }
}
