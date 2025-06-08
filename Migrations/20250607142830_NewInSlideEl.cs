using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPresentationApp.Migrations
{
    /// <inheritdoc />
    public partial class NewInSlideEl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlideElements_Slides_SlideId",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "LastEdited",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "Rotation",
                table: "SlideElements");

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                table: "SlideElements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SlideId",
                table: "SlideElements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionY",
                table: "SlideElements",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PositionX",
                table: "SlideElements",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "SlideElements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FontSize",
                table: "SlideElements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FontStyle",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FontWeight",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_SlideElements_Slides_SlideId",
                table: "SlideElements",
                column: "SlideId",
                principalTable: "Slides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlideElements_Slides_SlideId",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "FontSize",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "FontStyle",
                table: "SlideElements");

            migrationBuilder.DropColumn(
                name: "FontWeight",
                table: "SlideElements");

            migrationBuilder.AlterColumn<double>(
                name: "Width",
                table: "SlideElements",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SlideId",
                table: "SlideElements",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "PositionY",
                table: "SlideElements",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "PositionX",
                table: "SlideElements",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "SlideElements",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "SlideElements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEdited",
                table: "SlideElements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Rotation",
                table: "SlideElements",
                type: "float",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SlideElements_Slides_SlideId",
                table: "SlideElements",
                column: "SlideId",
                principalTable: "Slides",
                principalColumn: "Id");
        }
    }
}
