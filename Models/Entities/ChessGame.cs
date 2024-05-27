using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Entities
{
    public class ChessGame
    {
        public int Id { get; set; }
        public int WhitePlayerId { get; set; }
        public string WhitePlayerUsername { get; set; }
        public User WhitePlayer { get; set; }
        public string BlackPlayerUsername { get; set; }
        public int BlackPlayerId { get; set; }
        public User BlackPlayer { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public string Result { get; set; }
        public string GameEndReason { get; set; }
        public int Moves { get; set; }
        public int StartTime { get; set; }
        public int Increment { get; set; }
        public string PGN { get; set; }

        public ChessGame()
        {

        }
        public ChessGame(int whitePlayerId, string whitePlayerUsername, int blackPlayerId, string blackPlayerUsername,
                         DateTime dateStarted, DateTime dateFinished, string result, int startTime, int increment, string pgn, string gameEndReason, int moves)
        {
            WhitePlayerId = whitePlayerId;
            WhitePlayerUsername = whitePlayerUsername;
            BlackPlayerId = blackPlayerId;
            BlackPlayerUsername = blackPlayerUsername;
            DateStarted = dateStarted;
            DateFinished = dateFinished;
            Result = result;
            StartTime = startTime;
            Increment = increment;
            PGN = pgn;
            GameEndReason = gameEndReason;
            Moves = moves;
        }

        public ChessGame(LiveChessGame liveChessGame)
        {
            WhitePlayerId = liveChessGame.WhitePlayerId;
            WhitePlayerUsername = liveChessGame.WhitePlayerUsername;
            BlackPlayerId = liveChessGame.BlackPlayerId;
            BlackPlayerUsername = liveChessGame.BlackPlayerUsername;
            DateStarted = liveChessGame.DateStarted;
            DateFinished = DateTime.UtcNow;
            Result = liveChessGame.Result;
            GameEndReason = liveChessGame.GameEndReason;
            Moves = liveChessGame.MoveCount;
            StartTime = liveChessGame.StartTime;
            Increment = liveChessGame.Increment;
            PGN = liveChessGame.PGN;
        }
    }
}
