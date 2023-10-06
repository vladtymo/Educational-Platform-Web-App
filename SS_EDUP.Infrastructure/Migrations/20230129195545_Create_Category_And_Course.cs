using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SSEDUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCategoryAndCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Decription", "Name" },
                values: new object[,]
                {
                    { 1, null, "Programming" },
                    { 2, null, "UI/UX" },
                    { 3, null, "FrontEnd" },
                    { 4, null, "System programming" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "ImagePath", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Description C++ Basics", "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png", 900.0m, "C++ Basics" },
                    { 2, 1, "Description C++ Advanced", "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png", 910.0m, "C++ Advanced" },
                    { 3, 2, "Description Figma", "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png", 1500.0m, "Figma" },
                    { 4, 3, "Description HTML/CSS/JavaScript", "https://www.freeiconspng.com/thumbs/no-image-icon/no-image-icon-6.png", 1850.0m, "HTML/CSS/JavaScript" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
