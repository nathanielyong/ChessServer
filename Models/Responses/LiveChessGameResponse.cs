using Chess;
using ChessServer.Models.Entities;

namespace ChessServer.Models.Responses
{
    public class LiveChessGameResponse
    {
        public string Message { get; set; }
        public GameStateResponse GameState { get; set; }
        public class GameStateResponse { 
            public int Id { get; set; }
            public string WhitePlayerUsername { get; set; }
            public string BlackPlayerUsername { get; set; }
            public int StartTime { get; set; }
            public int Increment { get; set; }
            public DateTime DateStarted {  get; set; }
            public int WhiteTimeRemaining {  get; set; }
            public int BlackTimeRemaining {  get; set; }
            public bool IsWhiteTurn { get; set; }
            public string GameEndReason { get; set; }
            public string? Result { get; set; }
            public DateTime DateLastMove {  get; set; }
            public string? PrevMove { get; set; }
            public string CurrentPositionFEN {  get; set; }
            public string PGN { get; set; }
            public int MoveCount {  get; set; }
            public ICollection<string> LegalMoves { get; set; }

            public GameStateResponse(LiveChessGame liveChessGame)
            {
                Id = liveChessGame.Id;
                WhitePlayerUsername = liveChessGame.WhitePlayerUsername;
                BlackPlayerUsername = liveChessGame.BlackPlayerUsername;
                StartTime = (int)liveChessGame.StartTime.TotalMinutes;
                Increment = (int)liveChessGame.Increment.TotalSeconds;
                DateStarted = liveChessGame.DateStarted;
                TimeSpan time = DateTime.UtcNow - liveChessGame.DateLastMove;
                if (liveChessGame.MoveCount > 1 && liveChessGame.Result == "") { 
                    if (liveChessGame.IsWhiteTurn)
                    {
                        WhiteTimeRemaining = time > liveChessGame.WhiteTimeRemaining ? 0 : (int)(liveChessGame.WhiteTimeRemaining - time).TotalMilliseconds;
                        BlackTimeRemaining = (int)liveChessGame.BlackTimeRemaining.TotalMilliseconds;
                    }
                    else
                    {
                        WhiteTimeRemaining = (int)liveChessGame.WhiteTimeRemaining.TotalMilliseconds;
                        BlackTimeRemaining = time > liveChessGame.BlackTimeRemaining ? 0 : (int)(liveChessGame.BlackTimeRemaining - time).TotalMilliseconds;
                    }
                } 
                else
                {
                    WhiteTimeRemaining = (int)liveChessGame.WhiteTimeRemaining.TotalMilliseconds;
                    BlackTimeRemaining = (int)liveChessGame.BlackTimeRemaining.TotalMilliseconds;
                }
                IsWhiteTurn = liveChessGame.IsWhiteTurn;
                Result = liveChessGame.Result;
                GameEndReason = liveChessGame.GameEndReason;
                DateLastMove = liveChessGame.DateLastMove;
                PrevMove = liveChessGame.PrevMove;
                CurrentPositionFEN = liveChessGame.CurrentPositionFen;
                PGN = liveChessGame.PGN;
                MoveCount = liveChessGame.MoveCount;
                ChessBoard board = ChessBoard.LoadFromFen(CurrentPositionFEN);
                List<string> moves = new List<string>();
                foreach (var move in board.Moves())
                {
                    moves.Add(move.ToString());
                }
                LegalMoves = moves;
            }
        }

        public LiveChessGameResponse(LiveChessGame liveChessGame, string message)
        {
            Message = message;
            GameState = new GameStateResponse(liveChessGame);
        }
    }
}   
