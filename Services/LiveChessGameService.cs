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

        public bool CreateNewGame(string whitePlayerUsername,  string blackPlayerUsername, int startTime, int increment)
        {
            User whitePlayer = _userRepository.GetUserByUsername(whitePlayerUsername);
            User blackPlayer = _userRepository.GetUserByUsername(blackPlayerUsername);

            if (whitePlayer == null || blackPlayer == null || whitePlayer.LiveChessGameId != null || blackPlayer.LiveChessGameId != null || whitePlayerUsername == blackPlayerUsername)
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

        public LiveChessGameResponse ResignGame(string username)
        {
            LiveChessGame game = _liveChessGameRepository.GetLiveChessGameByUsername(username);
            if (game == null)
            {
                return null;
            }
            var colour_won = username == game.WhitePlayerUsername ? "Black" : "White";
            var result = username == game.WhitePlayerUsername ? "0-1" : "1-0";

            User whitePlayer = _userRepository.GetUserById(game.WhitePlayerId);
            User blackPlayer = _userRepository.GetUserById(game.BlackPlayerId);
            _liveChessGameRepository.FinishGame(game, result, $"{colour_won} won by resignation.");
            _chessGameRepository.CreateChessGame(new ChessGame(game), whitePlayer, blackPlayer);
            return new LiveChessGameResponse(game, $"Game has ended. {colour_won} won by resignation.");
        }

        public LiveChessGameResponse MakeMove(string username, string move_string)
        {
            LiveChessGame liveChessGame = _liveChessGameRepository.GetLiveChessGameByUsername(username);
            if (liveChessGame == null)
            {
                return null;
            }
            if (liveChessGame.Result != null)
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
            string[] pgn = board.ToPgn().Split(' ');
            string new_move = (liveChessGame.IsWhiteTurn ? $"{liveChessGame.MoveCount}. " : "") + pgn[pgn.Length - 1] + " ";
            if (liveChessGame.IsWhiteTurn) {
                string pgn_add = $"{liveChessGame.MoveCount}. ";
            }

            _liveChessGameRepository.UpdateLiveChessGame(liveChessGame, board.ToFen(), new_move, !liveChessGame.IsWhiteTurn);
            
            if (board.EndGame != null)
            {
                var result = board.EndGame.WonSide;
                User whitePlayer = _userRepository.GetUserById(liveChessGame.WhitePlayerId);
                User blackPlayer = _userRepository.GetUserById(liveChessGame.BlackPlayerId);
                if (result != null)
                {
                    _liveChessGameRepository.FinishGame(liveChessGame, result == PieceColor.White ? "1-0" : "0-1", $"{result.ToString} won by checkmate.");
                    _chessGameRepository.CreateChessGame(new ChessGame(liveChessGame), whitePlayer, blackPlayer);
                    return new LiveChessGameResponse(liveChessGame, $"Game has ended. {result.ToString} won by checkmate.");
                }
                else
                {
                    _liveChessGameRepository.FinishGame(liveChessGame, "1/2-1/2", $"Draw by {board.EndGame.EndgameType}");
                    _chessGameRepository.CreateChessGame(new ChessGame(liveChessGame), whitePlayer, blackPlayer);
                    return new LiveChessGameResponse(liveChessGame, $"Game has ended. Draw by {board.EndGame.EndgameType}");
                }
            }
            return new LiveChessGameResponse(liveChessGame, "Move executed successfully.");
        }
    }
}
