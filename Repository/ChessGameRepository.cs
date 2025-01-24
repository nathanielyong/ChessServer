using ChessServer.Data;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChessServer.Repository
{
    public class ChessGameRepository : IChessGameRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        public ChessGameRepository(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public ICollection<ChessGame> GetAllChessGames()
        {
            return _context.ChessGames.OrderBy(c => c.Id).ToList();
        }
        public ICollection<ChessGame> GetWhiteChessGamesByUsername(string username)
        {
            return _context.ChessGames
                    .Where(c => c.WhitePlayerUsername == username)
                    .ToList();
        }
        public ICollection<ChessGame> GetBlackChessGamesByUsername(string username)
        {
            return _context.ChessGames
                    .Where(c => c.BlackPlayerUsername == username)
                    .ToList();
        }
        public ICollection<ChessGame> GetChessGamesByUsername(string username)
        {
            return GetWhiteChessGamesByUsername(username).Concat(GetBlackChessGamesByUsername(username)).OrderBy(c => c.Id).ToList();
        }
        public ChessGame GetChessGameById(int id)
        {
            return _context.ChessGames.FirstOrDefault(c => c.Id == id);
        }
        public bool CreateChessGame(ChessGame chessGame)
        { 
            _context.Add(chessGame);
            User whitePlayer = chessGame.WhitePlayer;
            User blackPlayer = chessGame.BlackPlayer;
            whitePlayer.NumGamesPlayed += 1;
            blackPlayer.NumGamesPlayed += 1;
            if (chessGame.Result == "1-0")
            {
                whitePlayer.Wins += 1;
                blackPlayer.Losses += 1;
            } else if (chessGame.Result == "0-1")
            {
                whitePlayer.Losses += 1;
                blackPlayer.Wins += 1;
            } else
            {
                whitePlayer.Draws += 1;
                blackPlayer.Draws += 1;
            }
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
