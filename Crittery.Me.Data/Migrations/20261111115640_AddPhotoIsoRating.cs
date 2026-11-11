using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittery.Me.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoIsoRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Metadata_IsoSpeedRating",
                table: "Photos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Photos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata_IsoSpeedRating",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Photos");
        }
    }
}
