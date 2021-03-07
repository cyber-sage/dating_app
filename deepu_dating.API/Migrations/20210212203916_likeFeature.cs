using Microsoft.EntityFrameworkCore.Migrations;

namespace deepu_dating.API.Migrations
{
    public partial class likeFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userlike",
                columns: table => new
                {
                    sourceUserId = table.Column<int>(nullable: false),
                    likedUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userlike", x => new { x.sourceUserId, x.likedUserId });
                    table.ForeignKey(
                        name: "FK_userlike_userdata_likedUserId",
                        column: x => x.likedUserId,
                        principalTable: "userdata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userlike_userdata_sourceUserId",
                        column: x => x.sourceUserId,
                        principalTable: "userdata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userlike_likedUserId",
                table: "userlike",
                column: "likedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userlike");
        }
    }
}
