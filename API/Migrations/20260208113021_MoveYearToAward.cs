using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class MoveYearToAward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistAwards");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Awards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ArtistAward",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "int", nullable: false),
                    AwardsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistAward", x => new { x.ArtistsId, x.AwardsId });
                    table.ForeignKey(
                        name: "FK_ArtistAward_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistAward_Awards_AwardsId",
                        column: x => x.AwardsId,
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistAward_AwardsId",
                table: "ArtistAward",
                column: "AwardsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistAward");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Awards");

            migrationBuilder.CreateTable(
                name: "ArtistAwards",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    AwardId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistAwards", x => new { x.ArtistId, x.AwardId, x.Year });
                    table.ForeignKey(
                        name: "FK_ArtistAwards_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistAwards_Awards_AwardId",
                        column: x => x.AwardId,
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistAwards_AwardId",
                table: "ArtistAwards",
                column: "AwardId");
        }
    }
}
