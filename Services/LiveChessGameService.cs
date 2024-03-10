using Chess;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using ChessServer.Models.Responses;
using ChessServer.Repository;

namespace ChessServer.Services
{
    public class LiveChessGameService : ILiveChessGameService
    {
        private readonly IChessGameRepository _completedGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILiveChessGameRepository _liveChessGameRepository;
        public LiveChessGameService(IChessGameRepository completedGameRepository, IUserRepository userRepository, ILiveChessGameRepository liveChessGameRepository)
        {
            _completedGameRepository = completedGameRepository;
            _userRepository = userRepository;
            _liveChessGameRepository = liveChessGameRepository;
        }

        public bool CreateNewGame(string whitePlayerUsername,  string blackPlayerUsername, int startTime, int increment)
        {
            User whitePlayer = _userRepository.GetUserByUsername(whitePlayerUsername);
            User blackPlayer = _userRepository.GetUserByUsername(blackPlayerUsername);

            if (whitePlayer == null || blackPlayer == null || whitePlayer.LiveChessGameId != null || blackPlayer.LiveChessGameId != null)
            {
                return false;
            }

            LiveChessGame liveChessGame = new LiveChessGame(whitePlayer.Id, whitePlayerUsername, blackPlayer.Id, blackPlayerUsername, startTime, increment);
            if (_liveChessGameRepository.CreateLiveChessGame(liveChessGame, whitePlayer, blackPlayer)) { 
                return true;
            }
            return false;
        }
        public LiveChessGameResponse GetGameState(int gameId)
        {
            LiveChessGame game = _liveChessGameRepository.GetLiveChessGameById(gameId);
            if (game == null)
            {
                return null;
            }
            return new LiveChessGameResponse(game, "Current game state");
        }

        public LiveChessGameResponse GetGameState(string username)
        {

            LiveChessGame game = _liveChessGameRepository.GetLiveChessGameByUsername(username);
            if (game == null)
            {
                return null;
            }
            return new LiveChessGameResponse(game, "Current game state");
        }

        public LiveChessGameResponse MakeMove(string username, string move_string)
        {
            LiveChessGame liveChessGame = _liveChessGameRepository.GetLiveChessGameByUsername(username);
            if (liveChessGame == null)
            {
                return null;
            }
            if (liveChessGame.IsGameOver)
            {
                User user = _userRepository.GetUserByUsername(username);
                user.LiveChessGameId = null;
                return new LiveChessGameResponse(liveChessGame, liveChessGame.GameEndReason);
            }

            ChessBoard board = ChessBoard.LoadFromFen(liveChessGame.CurrentPositionFen);
            var player_colour = username == liveChessGame.WhitePlayerUsername ? PieceColor.White : PieceColor.Black;
            if (player_colour != board.Turn)
            {
                return new LiveChessGameResponse(liveChessGame, "It is not your turn.");
            }

            string[] move_squares = move_string.Split('-');
            var move = new Move(move_squares[0], move_squares[1]);
            if (!board.IsValidMove(move))
            {
                return new LiveChessGameResponse(liveChessGame, "Move is illegal.");
            }

            board.Move(move);
            System.Diagnostics.Debug.WriteLine(board.ToFen());
            _liveChessGameRepository.UpdateLiveChessGame(liveChessGame, board.ToFen(), board.ToPgn());

            if (board.EndGame != null)
            {
                if (board.EndGame.WonSide != null)
                {
                    _liveChessGameRepository.FinishGame(liveChessGame, $"{board.EndGame.WonSide} won by checkmate.");
                    return new LiveChessGameResponse(liveChessGame, $"Game has ended. {board.EndGame.WonSide} won by checkmate.");
                }
                else
                {
                    _liveChessGameRepository.FinishGame(liveChessGame, $"Draw by {board.EndGame.EndgameType}");
                    return new LiveChessGameResponse(liveChessGame, $"Game has ended. Draw by {board.EndGame.EndgameType}");
                }
            }
            return new LiveChessGameResponse(liveChessGame, "Move executed successfully.");
        }

    }
}
