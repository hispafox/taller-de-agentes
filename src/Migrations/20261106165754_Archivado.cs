using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class Archivado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archivada",
                table: "Incidencias",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archivada",
                table: "Incidencias");
        }
    }
}
