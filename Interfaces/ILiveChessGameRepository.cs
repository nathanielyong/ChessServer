using ChessServer.Models.Entities;
using Chess;

namespace ChessServer.Interfaces
{
    public interface ILiveChessGameRepository
    {
        LiveChessGame GetLiveChessGameById(int gameId);
        LiveChessGame GetLiveChessGameByUsername(string username);
        bool FinishGame(LiveChessGame liveChessGame, string gameEndReason);
        bool CreateLiveChessGame(LiveChessGame liveChessGame, User whitePlayer, User blackPlayer);
        bool UpdateLiveChessGame(LiveChessGame liveChessGame, string fen, string pgn);
        bool Save();
    }
}
