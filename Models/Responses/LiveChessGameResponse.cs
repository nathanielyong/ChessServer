using Chess;
using ChessServer.Models.Entities;

namespace ChessServer.Models.Responses
{
    public class LiveChessGameResponse
    {
        public string Message { get; set; }
        public GameStateResponse GameState { get; set; }
        public class GameStateResponse { 
            public string WhitePlayerUsername { get; set; }
            public string BlackPlayerUsername { get; set; }
            public int StartTime {  get; set; }
            public int Increment { get; set; }
            public DateTime DateStarted {  get; set; }
            public int WhiteMinutesRemaining {  get; set; }
            public int WhiteSecondsRemaining {  get; set; }
            public int BlackMinutesRemaining {  get; set; }
            public int BlackSecondsRemaining { get; set; }
            public bool IsWhiteTurn {  get; set; }
            public bool IsGameOver {  get; set; }
            public string GameEndReason { get; set; }
            public DateTime DateLastMove {  get; set; }
            public string CurrentPositionFEN {  get; set; }
            public string PGN { get; set; }
            public ICollection<string> LegalMoves { get; set; }

            public GameStateResponse(LiveChessGame liveChessGame)
            {
                WhitePlayerUsername = liveChessGame.WhitePlayerUsername;
                BlackPlayerUsername = liveChessGame.BlackPlayerUsername;
                StartTime = liveChessGame.StartTime;
                Increment = liveChessGame.Increment;
                DateStarted = liveChessGame.DateStarted;
                TimeSpan whiteTimeRemaining = liveChessGame.WhiteTimeRemaining;
                TimeSpan blackTimeRemaining = liveChessGame.BlackTimeRemaining;
                WhiteMinutesRemaining = whiteTimeRemaining.Minutes;
                WhiteSecondsRemaining = whiteTimeRemaining.Seconds;
                BlackMinutesRemaining = blackTimeRemaining.Minutes;
                BlackSecondsRemaining = blackTimeRemaining.Seconds;
                IsWhiteTurn = liveChessGame.IsWhiteTurn;
                IsGameOver = liveChessGame.IsGameOver;
                GameEndReason = liveChessGame.GameEndReason;
                DateLastMove = liveChessGame.DateLastMove;
                CurrentPositionFEN = liveChessGame.CurrentPositionFen;
                PGN = liveChessGame.PGN;
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
