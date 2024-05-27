using ChessServer.Models.Entities;

namespace ChessServer.Interfaces
{
    public interface IChessGameRepository
    {
        ICollection<ChessGame> GetAllChessGames();
        ICollection<ChessGame> GetWhiteChessGamesByUsername(string username);
        ICollection<ChessGame> GetBlackChessGamesByUsername(string username);
        ICollection<ChessGame> GetChessGamesByUsername(string username);
        ChessGame GetChessGameById(int id);
        bool CreateChessGame(ChessGame chessGame, User whitePlayer, User blackPlayer);
        bool Save();
    }
}
