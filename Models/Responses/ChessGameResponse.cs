using ChessServer.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Responses
{
    public class ChessGameResponse
    {
        public int Id { get; set; }
        public string WhitePlayerUsername { get; set; }
        public string BlackPlayerUsername { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public string Result { get; set; }
        public string GameEndReason {  get; set; }
        [Range(1, 120)]
        public int StartTime { get; set; }
        [Range(0, 60)]
        public int Increment { get; set; }
        public int Moves { get; set; }
        public string PGN { get; set; }
        public string FinalFEN { get; set; }

        public ChessGameResponse(ChessGame chessGame)
        {
            Id = chessGame.Id;
            WhitePlayerUsername = chessGame.WhitePlayerUsername;
            BlackPlayerUsername = chessGame.BlackPlayerUsername;
            DateStarted = chessGame.DateStarted;
            DateFinished = chessGame.DateFinished;
            PGN = chessGame.PGN;
            Result = chessGame.Result;
            GameEndReason = chessGame.GameEndReason;
            StartTime = chessGame.StartTime;
            Increment = chessGame.Increment;
            Moves = chessGame.Moves;
            FinalFEN = chessGame.FinalFEN;
        }
    }
}
