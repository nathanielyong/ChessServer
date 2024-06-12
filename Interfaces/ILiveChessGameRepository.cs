using ChessServer.Models.Entities;
using Chess;

namespace ChessServer.Interfaces
{
    public interface ILiveChessGameRepository
    {
        LiveChessGame GetLiveChessGameById(int? gameId);
        bool FinishGame(LiveChessGame liveChessGame, string result, string gameEndReason);
        bool UpdateGameTime(LiveChessGame liveChessGame, TimeSpan increment);
        bool CreateLiveChessGame(LiveChessGame liveChessGame);
        bool UpdateLiveChessGame(LiveChessGame liveChessGame, string fen, string new_move);
        bool Save();
    }
}
