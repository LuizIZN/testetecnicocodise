using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteTecnico.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimeImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Animes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Animes");
        }
    }
}
