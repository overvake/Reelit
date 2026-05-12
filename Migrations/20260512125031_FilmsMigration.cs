using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reelit.Migrations
{
    /// <inheritdoc />
    public partial class FilmsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Films");

            migrationBuilder.AlterColumn<string>(
                name: "ReleaseDate",
                table: "Films",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Films",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "Films",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "Films");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                table: "Films",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Films",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
