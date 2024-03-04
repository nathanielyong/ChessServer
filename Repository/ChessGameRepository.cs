using ChessServer.Data;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChessServer.Repository
{
    public class ChessGameRepository : IChessGameRepository
    {
        private readonly ApplicationDbContext _context;
        public ChessGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<ChessGame> GetChessGames()
        {
            return _context.ChessGames.OrderBy(c => c.Id).ToList();
        }
        public ICollection<ChessGame> GetWhiteChessGamesByUserId(int userId)
        {
            return _context.ChessGames
                    .Where(c => c.WhitePlayerId == userId)
                    .ToList();
        }
        public ICollection<ChessGame> GetBlackChessGamesByUserId(int userId)
        {
            return _context.ChessGames
                    .Where(c => c.BlackPlayerId == userId)
                    .ToList();
        }
        public ICollection<ChessGame> GetChessGamesByUserId(int userId)
        {
            return GetWhiteChessGamesByUserId(userId).Concat(GetBlackChessGamesByUserId(userId)).OrderBy(c => c.Id).ToList();
        }
        public ChessGame GetChessGameById(int id)
        {
            return _context.ChessGames.FirstOrDefault(c => c.Id == id);
        }
        public bool CreateChessGame(ChessGame chessGame)
        {
            _context.Add(chessGame);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
