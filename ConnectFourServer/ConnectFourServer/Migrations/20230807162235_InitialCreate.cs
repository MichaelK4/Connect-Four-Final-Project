using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectFourServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUsers",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChoiceID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GamesPlayed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsers", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "TblGames",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameStartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblGames", x => new { x.Username, x.GameId });
                    table.ForeignKey(
                        name: "FK_TblGames_TblUsers_Username",
                        column: x => x.Username,
                        principalTable: "TblUsers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblGames");

            migrationBuilder.DropTable(
                name: "TblUsers");
        }
    }
}
