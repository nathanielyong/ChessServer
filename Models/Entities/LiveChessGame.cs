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
        public TimeSpan StartTime { get; set; }
        public TimeSpan Increment { get; set; }
        public TimeSpan WhiteTimeRemaining {  get; set; }
        public TimeSpan BlackTimeRemaining { get; set; }
        public bool IsWhiteTurn { get; set; }
        public DateTime DateLastMove { get; set; }
        public DateTime DateLastUpdate { get; set; }
        public string? PrevMove {  get; set; }
        public string Result { get; set; }
        public string GameEndReason { get; set; }
        public string CurrentPositionFen {  get; set; }
        public string PGN { get; set; }
        public int MoveCount { get; set; }

        public LiveChessGame()
        { 
        }

        public LiveChessGame(int whitePlayerId, string whitePlayerUsername, int blackPlayerId, string blackPlayerUsername, TimeSpan startTime, TimeSpan increment)
        { 
            WhitePlayerId = whitePlayerId;
            WhitePlayerUsername = whitePlayerUsername;
            BlackPlayerId = blackPlayerId;
            BlackPlayerUsername = blackPlayerUsername;
            StartTime = startTime;
            Increment = increment;
            DateStarted = DateTime.UtcNow;
            WhiteTimeRemaining = startTime;
            BlackTimeRemaining = startTime;
            IsWhiteTurn = true;
            Result = "";
            GameEndReason = "";
            DateLastMove = DateStarted;
            DateLastUpdate = DateStarted;
            PrevMove = null;
            CurrentPositionFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            PGN = "";
            MoveCount = 1;
        }
    }
}
