using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace MoviesApi.Migrations
{
    public partial class cinemalocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Cinemas",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Cinemas");
        }
    }
}
