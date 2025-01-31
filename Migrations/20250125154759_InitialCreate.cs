using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiveChessGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhitePlayerId = table.Column<int>(type: "int", nullable: false),
                    WhitePlayerUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlackPlayerId = table.Column<int>(type: "int", nullable: false),
                    BlackPlayerUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Increment = table.Column<TimeSpan>(type: "time", nullable: false),
                    WhiteTimeRemaining = table.Column<TimeSpan>(type: "time", nullable: false),
                    BlackTimeRemaining = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsWhiteTurn = table.Column<bool>(type: "bit", nullable: false),
                    DateLastMove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevMove = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameEndReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPositionFen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PGN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoveCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveChessGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumGamesPlayed = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    Draws = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    LiveChessGameId = table.Column<int>(type: "int", nullable: true),
                    IsPlaying = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChessGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhitePlayerId = table.Column<int>(type: "int", nullable: false),
                    WhitePlayerUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlackPlayerUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlackPlayerId = table.Column<int>(type: "int", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GameEndReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Moves = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<int>(type: "int", nullable: false),
                    Increment = table.Column<int>(type: "int", nullable: false),
                    PGN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalFEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChessGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChessGames_Users_BlackPlayerId",
                        column: x => x.BlackPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChessGames_Users_WhitePlayerId",
                        column: x => x.WhitePlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_BlackPlayerId",
                table: "ChessGames",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChessGames_WhitePlayerId",
                table: "ChessGames",
                column: "WhitePlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChessGames");

            migrationBuilder.DropTable(
                name: "LiveChessGames");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
