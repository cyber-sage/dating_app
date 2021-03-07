using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace deepu_dating.API.Migrations
{
    public partial class apiExtensions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "userdata",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "userdata",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "userdata",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "userdata",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "userdata",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "userdata",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KnownAs",
                table: "userdata",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "userdata",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LookingFor",
                table: "userdata",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true),
                    IsMain = table.Column<bool>(nullable: false),
                    PublicId = table.Column<string>(nullable: true),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Photos_userdata_userId",
                        column: x => x.userId,
                        principalTable: "userdata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_userId",
                table: "Photos",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropColumn(
                name: "City",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "KnownAs",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "userdata");

            migrationBuilder.DropColumn(
                name: "LookingFor",
                table: "userdata");
        }
    }
}
