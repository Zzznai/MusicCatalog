using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPasswordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistAwards",
                table: "ArtistAwards");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistAwards",
                table: "ArtistAwards",
                columns: new[] { "ArtistId", "AwardId", "Year" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtistAwards",
                table: "ArtistAwards");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtistAwards",
                table: "ArtistAwards",
                columns: new[] { "ArtistId", "AwardId" });
        }
    }
}
