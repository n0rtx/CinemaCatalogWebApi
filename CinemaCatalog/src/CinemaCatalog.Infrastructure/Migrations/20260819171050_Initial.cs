using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaSearcherWithDb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Plot = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Director = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReleaseYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.CheckConstraint("CK__Movie__Director", "LEN(Director) > 0");
                    table.CheckConstraint("CK__Movie__Genre", "LEN(Genre) > 0");
                    table.CheckConstraint("CK__Movie__Plot", "LEN(Plot) > 0");
                    table.CheckConstraint("CK__Movie__ReleaseYear", "LEN(ReleaseYear) > 0");
                    table.CheckConstraint("CK__Movie__Title", "LEN(Title) > 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
