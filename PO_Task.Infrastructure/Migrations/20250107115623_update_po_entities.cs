using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PO_Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_po_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "PO",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "status_value",
                schema: "PO",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 110,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "status_name",
                schema: "PO",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Created",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                schema: "PO",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deactivated",
                schema: "PO",
                table: "PurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                schema: "PO",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "is_deactivated",
                schema: "PO",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<int>(
                name: "status_value",
                schema: "PO",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 110);

            migrationBuilder.AlterColumn<string>(
                name: "status_name",
                schema: "PO",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Created");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                schema: "PO",
                table: "OrderItems",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
