using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using ChessServer.Models.Requests;
using ChessServer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ChessServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessGameController : Controller
    {
        private readonly IChessGameRepository _chessGameRepository;
        private readonly IUserRepository _userRepository;
        public ChessGameController(IChessGameRepository chessGameRepository, IUserRepository userRepository)
        {
            _chessGameRepository = chessGameRepository;
            _userRepository = userRepository;
        }

        [HttpGet("getChessGames")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetChessGames()
        {
            var chessGames = _chessGameRepository.GetChessGames();
            if (chessGames == null || !chessGames.Any())
            {
                return NotFound();
            }

            return Ok(chessGames);
        }

        [HttpGet("user/{userId:int}/whiteGames")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetWhiteChessGamesByUserId(int userId)
        {
            var chessGames = _chessGameRepository.GetWhiteChessGamesByUserId(userId);
            if (chessGames == null || !chessGames.Any())
            {
                return NotFound();
            }

            return Ok(chessGames);
        }

        [HttpGet("user/{userId:int}/blackGames")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetBlackChessGamesByUserId(int userId)
        {
            var chessGames = _chessGameRepository.GetBlackChessGamesByUserId(userId);
            if (chessGames == null || !chessGames.Any())
            {
                return NotFound();
            }
             
            return Ok(chessGames);
        }

        [HttpGet("user/{userId:int}/games")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetChessGamesByUserId(int userId)
        {
            var chessGames = _chessGameRepository.GetChessGamesByUserId(userId);
            if (chessGames == null || !chessGames.Any())
            {
                return NotFound();
            }

            return Ok(chessGames);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetChessGameById(int id)
        {
            var chessGame = _chessGameRepository.GetChessGameById(id);
            if (chessGame == null)
            {
                return NotFound();
            }

            return Ok(chessGame);
        }

        [HttpPost("createGame")]
        public IActionResult CreateChessGame(CreateChessGameRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(errors);
            }
            User whitePlayer = _userRepository.GetUserByUsername(request.WhitePlayerUsername);
            User blackPlayer = _userRepository.GetUserByUsername(request.BlackPlayerUsername);

            if (whitePlayer == null || blackPlayer == null)
            {
                return BadRequest("A username was not found");
            }

            ChessGame chessGame = new ChessGame(whitePlayer.Id, whitePlayer.Username, blackPlayer.Id, blackPlayer.Username,
                request.DateStarted, request.DateFinished, request.Result, new TimeSpan(0, request.StartTime, 0), new TimeSpan(0, 0, request.Increment), request.PGN);
            if (_chessGameRepository.CreateChessGame(chessGame))
            {
                return Ok(chessGame);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
