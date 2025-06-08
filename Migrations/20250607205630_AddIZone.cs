using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPresentationApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZIndex",
                table: "SlideElements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZIndex",
                table: "SlideElements");
        }
    }
}
