using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp6.Migrations
{
    /// <inheritdoc />
    public partial class _15mc696 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostItem_Users_UserId1",
                table: "LostItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LostItem",
                table: "LostItem");

            migrationBuilder.DropIndex(
                name: "IX_LostItem_UserId1",
                table: "LostItem");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "LostItem");

            migrationBuilder.RenameTable(
                name: "LostItem",
                newName: "LostItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LostItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LostItems",
                table: "LostItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LostItems_UserId",
                table: "LostItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LostItems_Users_UserId",
                table: "LostItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostItems_Users_UserId",
                table: "LostItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LostItems",
                table: "LostItems");

            migrationBuilder.DropIndex(
                name: "IX_LostItems_UserId",
                table: "LostItems");

            migrationBuilder.RenameTable(
                name: "LostItems",
                newName: "LostItem");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LostItem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "LostItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LostItem",
                table: "LostItem",
                column: "Id");

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
    }
}
