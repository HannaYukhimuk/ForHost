using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPresentationApp.Migrations
{
    /// <inheritdoc />
    public partial class ColAndBord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BorderColor",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BorderWidth",
                table: "SlideElements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FillColor",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorderColor",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "BorderWidth",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "FillColor",
                table: "SlideElements");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
