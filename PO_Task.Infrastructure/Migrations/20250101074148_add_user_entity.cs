using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PO_Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_user_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_profile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profile", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profile_user_id",
                        column: x => x.id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "user",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_profile_email",
                table: "user_profile",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_profile");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
