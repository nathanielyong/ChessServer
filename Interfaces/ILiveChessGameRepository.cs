using ChessServer.Models.Entities;
using Chess;

namespace ChessServer.Interfaces
{
    public interface ILiveChessGameRepository
    {
        LiveChessGame GetLiveChessGameById(int gameId);
        LiveChessGame GetLiveChessGameByUsername(string username);
        bool FinishGame(LiveChessGame liveChessGame, string result, string gameEndReason);
        bool CreateLiveChessGame(LiveChessGame liveChessGame, User whitePlayer, User blackPlayer);
        bool UpdateLiveChessGame(LiveChessGame liveChessGame, string fen, string new_move, bool increment_turn);
        bool Save();
    }
}
