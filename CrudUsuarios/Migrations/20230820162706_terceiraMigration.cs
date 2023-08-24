using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudUsuarios.Migrations
{
    /// <inheritdoc />
    public partial class terceiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
