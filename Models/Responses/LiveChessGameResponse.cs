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
            public int StartTime {  get; set; }
            public int Increment { get; set; }
            public DateTime DateStarted {  get; set; }
            public int WhiteMinutesRemaining {  get; set; }
            public int WhiteSecondsRemaining {  get; set; }
            public int BlackMinutesRemaining {  get; set; }
            public int BlackSecondsRemaining { get; set; }
            public bool IsWhiteTurn { get; set; }
            public string GameEndReason { get; set; }
            public string? Result { get; set; }
            public DateTime DateLastMove {  get; set; }
            public string? PrevMove {  get; set; }
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
                TimeSpan whiteTimeRemaining = liveChessGame.WhiteTimeRemaining;
                TimeSpan blackTimeRemaining = liveChessGame.BlackTimeRemaining;
                WhiteMinutesRemaining = whiteTimeRemaining.Minutes;
                WhiteSecondsRemaining = whiteTimeRemaining.Seconds;
                BlackMinutesRemaining = blackTimeRemaining.Minutes;
                BlackSecondsRemaining = blackTimeRemaining.Seconds;
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
