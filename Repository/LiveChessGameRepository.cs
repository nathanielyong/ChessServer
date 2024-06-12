using Chess;
using ChessServer.Data;
using ChessServer.Interfaces;
using ChessServer.Models.Entities;

namespace ChessServer.Repository
{
    public class LiveChessGameRepository : ILiveChessGameRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IChessGameRepository _chessGameRepository;
        public LiveChessGameRepository(ApplicationDbContext context, IUserRepository userRepository, IChessGameRepository chessGameRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _chessGameRepository = chessGameRepository;
        }
        public LiveChessGame GetLiveChessGameById(int? gameId)
        {
            if (gameId == null)
            {
                return null;
            }
            LiveChessGame liveChessGame = _context.LiveChessGames.FirstOrDefault(c => c.Id == gameId);
            if (liveChessGame == null)
            {
                return null;
            }
            UpdateGameTime(liveChessGame, TimeSpan.Zero);
            return liveChessGame;
        }
        public bool UpdateGameTime(LiveChessGame liveChessGame, TimeSpan increment)
        {
            if (liveChessGame.Result == "") { 
                TimeSpan moveTime = DateTime.UtcNow - liveChessGame.DateLastUpdate;
                if (liveChessGame.IsWhiteTurn)
                {
                    if (moveTime > liveChessGame.WhiteTimeRemaining)
                    {
                        liveChessGame.WhiteTimeRemaining = TimeSpan.Zero;
                        FinishGame(liveChessGame, "0-1", "Black wins on time.");
                    }
                    else
                    {
                        TimeSpan newTimeSpan = liveChessGame.WhiteTimeRemaining.Subtract(moveTime);
                        liveChessGame.WhiteTimeRemaining = newTimeSpan.Add(increment);
                    }
                }
                else
                {
                    if (moveTime > liveChessGame.BlackTimeRemaining)
                    {
                        liveChessGame.BlackTimeRemaining = TimeSpan.Zero;
                        FinishGame(liveChessGame, "1-0", "White wins on time.");
                    }
                    else
                    {
                        TimeSpan newTimeSpan = liveChessGame.BlackTimeRemaining.Subtract(moveTime);
                        liveChessGame.BlackTimeRemaining = newTimeSpan.Add(increment);
                    }
                }
                liveChessGame.DateLastUpdate = DateTime.UtcNow;
            }
            return Save();
        }
        public bool FinishGame(LiveChessGame liveChessGame, string result, string gameEndReason)
        {
            if (liveChessGame.Result != "")
            {
                return false;
            }
            User whitePlayer = _userRepository.GetUserById(liveChessGame.WhitePlayerId);
            User blackPlayer = _userRepository.GetUserById(liveChessGame.BlackPlayerId);
            liveChessGame.Result = result;
            liveChessGame.PGN += result;
            liveChessGame.GameEndReason = gameEndReason;
            whitePlayer.IsPlaying = false;
            blackPlayer.IsPlaying = false;
            _chessGameRepository.CreateChessGame(new ChessGame(liveChessGame));
            return Save();
        }
        public bool CreateLiveChessGame(LiveChessGame liveChessGame)
        {
            _context.Add(liveChessGame);
            if (!Save())
            {
                return false;
            }
            User whitePlayer = _userRepository.GetUserById(liveChessGame.WhitePlayerId);
            User blackPlayer = _userRepository.GetUserById(liveChessGame.BlackPlayerId);
            whitePlayer.LiveChessGameId = liveChessGame.Id;
            blackPlayer.LiveChessGameId = liveChessGame.Id;
            whitePlayer.IsPlaying = true;
            blackPlayer.IsPlaying = true;
            return Save();
        }

        public bool UpdateLiveChessGame(LiveChessGame liveChessGame, string fen, string new_move)
        {
            UpdateGameTime(liveChessGame, liveChessGame.Increment);
            liveChessGame.CurrentPositionFen = fen;
            liveChessGame.PrevMove = new_move;
            liveChessGame.PGN += new_move;
            liveChessGame.IsWhiteTurn = !liveChessGame.IsWhiteTurn;
            liveChessGame.DateLastMove = DateTime.UtcNow;
            if (liveChessGame.IsWhiteTurn)
            {
                liveChessGame.MoveCount++;
            }
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
