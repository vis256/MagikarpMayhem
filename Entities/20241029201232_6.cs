using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagikarpMayhem.Entities
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonType",
                table: "PokemonType");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PokemonType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonType",
                table: "PokemonType",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonType",
                table: "PokemonType");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PokemonType",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonType",
                table: "PokemonType",
                column: "Id");
        }
    }
}
