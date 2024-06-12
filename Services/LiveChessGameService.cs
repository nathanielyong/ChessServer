using Chess;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using ChessServer.Models.Responses;
using ChessServer.Repository;

namespace ChessServer.Services
{
    public class LiveChessGameService : ILiveChessGameService
    {
        private readonly IChessGameRepository _chessGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILiveChessGameRepository _liveChessGameRepository;
        public LiveChessGameService(IChessGameRepository chessGameRepository, IUserRepository userRepository, ILiveChessGameRepository liveChessGameRepository)
        {
            _chessGameRepository = chessGameRepository;
            _userRepository = userRepository;
            _liveChessGameRepository = liveChessGameRepository;
        }

        public bool CreateNewGame(string whitePlayerUsername, string blackPlayerUsername, int startTime, int increment)
        {
            User whitePlayer = _userRepository.GetUserByUsername(whitePlayerUsername);
            User blackPlayer = _userRepository.GetUserByUsername(blackPlayerUsername);

            if (whitePlayer == null || blackPlayer == null || whitePlayer.IsPlaying || blackPlayer.IsPlaying || whitePlayerUsername == blackPlayerUsername)
            {
                return false;
            }
            TimeSpan startTimeSpan = TimeSpan.FromMinutes(startTime);
            TimeSpan incrementTimeSpan = TimeSpan.FromSeconds(increment);
            LiveChessGame liveChessGame = new LiveChessGame(whitePlayer.Id, whitePlayerUsername, blackPlayer.Id, blackPlayerUsername, startTimeSpan, incrementTimeSpan);
            if (_liveChessGameRepository.CreateLiveChessGame(liveChessGame)) { 
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
            User player = _userRepository.GetUserByUsername(username);
            LiveChessGame game = _liveChessGameRepository.GetLiveChessGameById(player.LiveChessGameId);
            if (game == null)
            {
                return null;
            }
            return new LiveChessGameResponse(game, "Current game state");
        }

        public LiveChessGameResponse ResignGame(string username)
        {
            User player = _userRepository.GetUserByUsername(username);
            LiveChessGame game = _liveChessGameRepository.GetLiveChessGameById(player.LiveChessGameId);
            if (game == null)
            {
                return null;
            }
            var colour_won = username == game.WhitePlayerUsername ? "Black" : "White";
            var result = username == game.WhitePlayerUsername ? "0-1" : "1-0";
            _liveChessGameRepository.UpdateGameTime(game, TimeSpan.Zero);
            _liveChessGameRepository.FinishGame(game, result, $"{colour_won} won by resignation.");
            return new LiveChessGameResponse(game, $"Game has ended. {game.GameEndReason}");
        }

        public LiveChessGameResponse MakeMove(string username, string move_string)
        {
            User player = _userRepository.GetUserByUsername(username);
            LiveChessGame liveChessGame = _liveChessGameRepository.GetLiveChessGameById(player.LiveChessGameId);
            if (liveChessGame == null)
            {
                return null;
            }
            if (liveChessGame.Result != "")
            {
                player.LiveChessGameId = null;
                return new LiveChessGameResponse(liveChessGame, liveChessGame.GameEndReason);
            }

            ChessBoard board = ChessBoard.LoadFromFen(liveChessGame.CurrentPositionFen);
            var player_colour = username == liveChessGame.WhitePlayerUsername ? PieceColor.White : PieceColor.Black;
            if (player_colour != board.Turn)
            {
                return new LiveChessGameResponse(liveChessGame, "It is not your turn.");
            }

            string[] move_squares = move_string.Split('-');
            if (move_squares.Length != 2)
            {
                return new LiveChessGameResponse(liveChessGame, "Move format invalid.");
            }
            var move = new Move(move_squares[0], move_squares[1]);
            try
            {
                if (!board.IsValidMove(move))
                {
                    return new LiveChessGameResponse(liveChessGame, "Move is illegal.");
                }
            }
            catch (Exception e)
            {
                return new LiveChessGameResponse(liveChessGame, "Move is illegal.");
            }

            board.Move(move);
            string new_move = (liveChessGame.IsWhiteTurn ? $"{liveChessGame.MoveCount}. " : "") + board.MovesToSan[board.MovesToSan.Count - 1] + " ";
            _liveChessGameRepository.UpdateLiveChessGame(liveChessGame, board.ToFen(), new_move);
            
            if (board.EndGame != null)
            {
                var result = board.EndGame.WonSide;
                if (result != null)
                {
                    _liveChessGameRepository.FinishGame(liveChessGame, result == PieceColor.White ? "1-0" : "0-1", $"{result} won by checkmate.");
                    return new LiveChessGameResponse(liveChessGame, $"Game has ended. {liveChessGame.GameEndReason}");
                }
                else
                {
                    _liveChessGameRepository.FinishGame(liveChessGame, "1/2-1/2", $"Draw by {board.EndGame.EndgameType}");
                    return new LiveChessGameResponse(liveChessGame, $"Game has ended. {liveChessGame.GameEndReason}");
                }
            }
            return new LiveChessGameResponse(liveChessGame, "Move executed successfully.");
        }
    }
}
