using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PO_Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_user_entity_configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_profile_user_id",
                table: "user_profile");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "user_profile",
                newName: "profile_last_name");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "user_profile",
                newName: "profile_first_name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "user_profile",
                newName: "profile_email");

            migrationBuilder.RenameIndex(
                name: "ix_user_profile_email",
                table: "user_profile",
                newName: "ix_user_profile_profile_email");

            migrationBuilder.CreateIndex(
                name: "ix_user_profile_id",
                table: "user_profile",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_profile_id",
                table: "user_profile");

            migrationBuilder.RenameColumn(
                name: "profile_last_name",
                table: "user_profile",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "profile_first_name",
                table: "user_profile",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "profile_email",
                table: "user_profile",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "ix_user_profile_profile_email",
                table: "user_profile",
                newName: "ix_user_profile_email");

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

            migrationBuilder.CreateIndex(
                name: "ix_user_id",
                table: "user",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_profile_user_id",
                table: "user_profile",
                column: "id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
