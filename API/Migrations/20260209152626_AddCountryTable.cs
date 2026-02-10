using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasedIn",
                table: "RecordLabels");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "RecordLabels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordLabels_CountryId",
                table: "RecordLabels",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabels_Countries_CountryId",
                table: "RecordLabels",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabels_Countries_CountryId",
                table: "RecordLabels");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_RecordLabels_CountryId",
                table: "RecordLabels");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "RecordLabels");

            migrationBuilder.AddColumn<string>(
                name: "BasedIn",
                table: "RecordLabels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
