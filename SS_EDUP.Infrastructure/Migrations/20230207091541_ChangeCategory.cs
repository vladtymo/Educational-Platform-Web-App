using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSEDUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Categories",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "Decription");
        }
    }
}
