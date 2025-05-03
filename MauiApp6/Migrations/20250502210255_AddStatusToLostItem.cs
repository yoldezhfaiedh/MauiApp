using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp6.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToLostItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LostItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "LostItems");
        }
    }
}
