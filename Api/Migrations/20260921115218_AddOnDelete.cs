using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteTecnico.Migrations
{
    /// <inheritdoc />
    public partial class AddOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Diretores_DiretorId",
                table: "Animes");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Diretores_DiretorId",
                table: "Animes",
                column: "DiretorId",
                principalTable: "Diretores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Diretores_DiretorId",
                table: "Animes");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Diretores_DiretorId",
                table: "Animes",
                column: "DiretorId",
                principalTable: "Diretores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
