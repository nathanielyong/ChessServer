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
        public Result Result { get; set; }
        public int StartTime { get; set; }
        public int Increment { get; set; }
        public string PGN { get; set; }

        public ChessGame()
        {

        }
        public ChessGame(int whitePlayerId, string whitePlayerUsername, int blackPlayerId, string blackPlayerUsername,
                         DateTime dateStarted, DateTime dateFinished, Result result, int startTime, int increment, string pgn)
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
        }
    }
}
