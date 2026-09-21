using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteTecnico.Migrations
{
    /// <inheritdoc />
    public partial class AddDatetimeDataNascimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Diretores"
                ALTER COLUMN "DataNascimento" TYPE date
                USING "DataNascimento"::date;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Diretores"
                ALTER COLUMN "DataNascimento" TYPE text
                USING "DataNascimento"::text;
                """);
        }
    }
}
