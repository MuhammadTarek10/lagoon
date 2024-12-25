using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeIdCapital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Villas",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Villas",
                newName: "id");
        }
    }
}
