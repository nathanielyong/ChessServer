using ChessServer.Models.Responses;

namespace ChessServer.Interfaces
{
    public interface ILiveChessGameService
    {
        bool CreateNewGame(string WhitePlayerUsername, string BlackPlayerUsername, int startTime, int increment);
        LiveChessGameResponse GetGameState(int gameId);
        LiveChessGameResponse GetGameState(string username);
        LiveChessGameResponse MakeMove(string username, string move);
        LiveChessGameResponse ResignGame(string username);
    }
}
