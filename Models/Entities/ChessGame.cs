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
        public string FinalFEN {  get; set; }

        public ChessGame()
        {

        }
        public ChessGame(int whitePlayerId, string whitePlayerUsername, int blackPlayerId, string blackPlayerUsername,
                         DateTime dateStarted, DateTime dateFinished, string result, int startTime, int increment, string pgn, string gameEndReason, int moves, string finalFEN)
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
            FinalFEN = finalFEN;
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
            Moves = liveChessGame.MoveCount - (liveChessGame.IsWhiteTurn ? 1 : 0);
            StartTime = liveChessGame.StartTime.Minutes;
            Increment = liveChessGame.Increment.Seconds;
            PGN = liveChessGame.PGN;
            FinalFEN = liveChessGame.CurrentPositionFen;
        }
    }
}
