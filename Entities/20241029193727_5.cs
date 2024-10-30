using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagikarpMayhem.Entities
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondTypeId",
                table: "Pokemon");

            migrationBuilder.RenameColumn(
                name: "FirstTypeId",
                table: "Pokemon",
                newName: "PokedexNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PokedexNumber",
                table: "Pokemon",
                newName: "FirstTypeId");

            migrationBuilder.AddColumn<int>(
                name: "SecondTypeId",
                table: "Pokemon",
                type: "INTEGER",
                nullable: true);
        }
    }
}
