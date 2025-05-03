using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp6.Migrations
{
    /// <inheritdoc />
    public partial class _15mc6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LostItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "LostItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LostItem_UserId1",
                table: "LostItem",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LostItem_Users_UserId1",
                table: "LostItem",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostItem_Users_UserId1",
                table: "LostItem");

            migrationBuilder.DropIndex(
                name: "IX_LostItem_UserId1",
                table: "LostItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LostItem");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "LostItem");
        }
    }
}
