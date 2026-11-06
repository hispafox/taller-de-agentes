using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zonas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zonas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true),
                    Prioridad = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    FechaApertura = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    FechaResolucion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FechaCierre = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ZonaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidencias_Zonas_ZonaId",
                        column: x => x.ZonaId,
                        principalTable: "Zonas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_ZonaId",
                table: "Incidencias",
                column: "ZonaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidencias");

            migrationBuilder.DropTable(
                name: "Zonas");
        }
    }
}
