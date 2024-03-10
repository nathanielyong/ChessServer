using ChessServer.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Responses
{
    public class ChessGameResponse
    {
        public string WhitePlayerUsername { get; set; }
        public string BlackPlayerUsername { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public Result Result { get; set; }
        [Range(1, 120)]
        public int StartTime { get; set; }
        [Range(0, 60)]
        public int Increment { get; set; }
        public string PGN { get; set; }

        public ChessGameResponse(ChessGame chessGame)
        {
            WhitePlayerUsername = chessGame.WhitePlayerUsername;
            BlackPlayerUsername = chessGame.BlackPlayerUsername;
            DateStarted = chessGame.DateStarted;
            DateFinished = chessGame.DateFinished;
            PGN = chessGame.PGN;
            Result = chessGame.Result;
            StartTime = chessGame.StartTime;
            Increment = chessGame.Increment;
        }
    }
}
