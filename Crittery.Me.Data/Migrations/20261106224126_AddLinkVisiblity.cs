using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittery.Me.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkVisiblity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Visiblity",
                table: "Links",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visiblity",
                table: "Links");
        }
    }
}
