using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteTecnico.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimeList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiretorId1",
                table: "Animes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animes_DiretorId1",
                table: "Animes",
                column: "DiretorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Diretores_DiretorId1",
                table: "Animes",
                column: "DiretorId1",
                principalTable: "Diretores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Diretores_DiretorId1",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_DiretorId1",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "DiretorId1",
                table: "Animes");
        }
    }
}
