using Chess;
using ChessServer.Data;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;

namespace ChessServer.Repository
{
    public class LiveChessGameRepository : ILiveChessGameRepository
    {
        private readonly ApplicationDbContext _context;
        public LiveChessGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public LiveChessGame GetLiveChessGameById(int gameId)
        {
            return _context.LiveChessGames.FirstOrDefault(c => c.Id == gameId);
        }
        public LiveChessGame GetLiveChessGameByUsername(string username)
        {
            return _context.LiveChessGames.FirstOrDefault(c => c.WhitePlayerUsername == username || c.BlackPlayerUsername == username);
        }
        public bool FinishGame(LiveChessGame game, string gameEndReason)
        {
            game.IsGameOver = true;
            game.GameEndReason = gameEndReason;
            return Save();
        }
        public bool CreateLiveChessGame(LiveChessGame chessGame, User whitePlayer, User blackPlayer)
        {
            _context.Add(chessGame);
            whitePlayer.LiveChessGameId = chessGame.Id;
            blackPlayer.LiveChessGameId = chessGame.Id;
            return Save();

        }

        public bool UpdateLiveChessGame(LiveChessGame liveChessGame, string fen, string pgn)
        {
            liveChessGame.CurrentPositionFen = fen;
            liveChessGame.PGN = liveChessGame.PGN + pgn;
            liveChessGame.IsWhiteTurn = !liveChessGame.IsWhiteTurn;
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
