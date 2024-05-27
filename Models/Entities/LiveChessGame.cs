using Chess;
using System.Text.Json;

namespace ChessServer.Models.Entities
{
    public class LiveChessGame
    { 
        public int Id { get; set; }
        public int WhitePlayerId { get; set; }
        public string WhitePlayerUsername { get; set; }
        public int BlackPlayerId { get; set; }
        public string BlackPlayerUsername { get; set; }
        public DateTime DateStarted { get; set; }
        public int StartTime { get; set; }
        public int Increment { get; set; }
        public TimeSpan WhiteTimeRemaining {  get; set; }
        public TimeSpan BlackTimeRemaining { get; set; }
        public bool IsWhiteTurn { get; set; }
        public DateTime DateLastMove { get; set; }
        public string? Result { get; set; }
        public string? GameEndReason { get; set; }
        public string CurrentPositionFen {  get; set; }
        public string PGN { get; set; }
        public int MoveCount { get; set; }

        public LiveChessGame()
        { 
        }

        public LiveChessGame(int whitePlayerId, string whitePlayerUsername, int blackPlayerId, string blackPlayerUsername, int startTime, int increment)
        { 
            WhitePlayerId = whitePlayerId;
            WhitePlayerUsername = whitePlayerUsername;
            BlackPlayerId = blackPlayerId;
            BlackPlayerUsername = blackPlayerUsername;
            StartTime = startTime;
            Increment = increment;
            DateStarted = DateTime.UtcNow;
            WhiteTimeRemaining = new TimeSpan(0, startTime, 0);
            BlackTimeRemaining = new TimeSpan(0, startTime, 0);
            IsWhiteTurn = true;
            Result = null;
            DateLastMove = DateStarted;
            CurrentPositionFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            PGN = "";
            MoveCount = 1;
        }
    }
}
