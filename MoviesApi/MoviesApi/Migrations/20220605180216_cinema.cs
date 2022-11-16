using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesApi.Migrations
{
    public partial class cinema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "ActorId", "MovieId" },
                keyValues: new object[] { 500, 400 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "ActorId", "MovieId" },
                keyValues: new object[] { 600, 200 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "ActorId", "MovieId" },
                keyValues: new object[] { 600, 300 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "ActorId", "MovieId" },
                keyValues: new object[] { 700, 200 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "ActorId", "MovieId" },
                keyValues: new object[] { 700, 300 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 100, 200 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 100, 300 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 100, 400 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 100, 600 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 300, 200 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 300, 300 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 300, 500 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 300, 600 });

            migrationBuilder.DeleteData(
                table: "MoviesGenders",
                keyColumns: new[] { "GenderId", "MovieId" },
                keyValues: new object[] { 400, 500 });

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 600);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 700);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 600);

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cinemas_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MoviesCinema",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesCinema", x => new { x.CinemaId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MoviesCinema_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesCinema_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cinemas_CinemaId",
                table: "Cinemas",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesCinema_MovieId",
                table: "MoviesCinema",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesCinema");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "BirthDate", "Name", "Photo" },
                values: new object[,]
                {
                    { 500, new DateTime(1962, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jim Carrey", null },
                    { 600, new DateTime(1965, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Robert Downey Jr.", null },
                    { 700, new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chris Evans", null }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 100, "Aventura" },
                    { 200, "Animación" },
                    { 300, "Suspenso" },
                    { 400, "Romance" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AtCinema", "Poster", "ReleaseDat", "Title" },
                values: new object[,]
                {
                    { 200, true, null, new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers: Endgame" },
                    { 300, false, null, new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers: Infinity Wars" },
                    { 400, false, null, new DateTime(2020, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sonic the Hedgehog" },
                    { 500, false, null, new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emma" },
                    { 600, false, null, new DateTime(2020, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wonder Woman 1984" }
                });

            migrationBuilder.InsertData(
                table: "MoviesActors",
                columns: new[] { "ActorId", "MovieId", "Character", "Order" },
                values: new object[,]
                {
                    { 600, 200, "Tony Stark", 1 },
                    { 700, 200, "Steve Rogers", 2 },
                    { 600, 300, "Tony Stark", 1 },
                    { 700, 300, "Steve Rogers", 2 },
                    { 500, 400, "Dr. Ivo Robotnik", 1 }
                });

            migrationBuilder.InsertData(
                table: "MoviesGenders",
                columns: new[] { "GenderId", "MovieId" },
                values: new object[,]
                {
                    { 300, 200 },
                    { 100, 200 },
                    { 300, 300 },
                    { 100, 300 },
                    { 100, 400 },
                    { 300, 500 },
                    { 400, 500 },
                    { 300, 600 },
                    { 100, 600 }
                });
        }
    }
}
