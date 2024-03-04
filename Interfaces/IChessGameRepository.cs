using ChessServer.Models.Entities;

namespace ChessServer.Interfaces
{
    public interface IChessGameRepository
    {
        ICollection<ChessGame> GetChessGames();
        ICollection<ChessGame> GetWhiteChessGamesByUserId(int userId);
        ICollection<ChessGame> GetBlackChessGamesByUserId(int userId);
        ICollection<ChessGame> GetChessGamesByUserId(int userId);
        ChessGame GetChessGameById(int id);
        bool CreateChessGame(ChessGame chessGame);
        bool Save();
    }
}
