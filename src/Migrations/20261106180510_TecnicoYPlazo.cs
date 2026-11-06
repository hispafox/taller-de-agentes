using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class TecnicoYPlazo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Plazo",
                table: "Incidencias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TecnicoId",
                table: "Incidencias",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnicos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_TecnicoId",
                table: "Incidencias",
                column: "TecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidencias_Tecnicos_TecnicoId",
                table: "Incidencias",
                column: "TecnicoId",
                principalTable: "Tecnicos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidencias_Tecnicos_TecnicoId",
                table: "Incidencias");

            migrationBuilder.DropTable(
                name: "Tecnicos");

            migrationBuilder.DropIndex(
                name: "IX_Incidencias_TecnicoId",
                table: "Incidencias");

            migrationBuilder.DropColumn(
                name: "Plazo",
                table: "Incidencias");

            migrationBuilder.DropColumn(
                name: "TecnicoId",
                table: "Incidencias");
        }
    }
}
