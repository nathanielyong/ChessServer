using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using ChessServer.Models.Requests;
using ChessServer.Models.Responses;
using ChessServer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("myGames"), Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetMyGames()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var chessGames = _chessGameRepository.GetChessGamesByUsername(username);
            List<ChessGameResponse> response = new List<ChessGameResponse>();
            foreach (var chessGame in chessGames)
            {
                var chessGameResponse = new ChessGameResponse(chessGame);
                response.Add(chessGameResponse);
            }
            return Ok(response);
        }

        [HttpGet("user/{username}/whiteGames")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetWhiteChessGamesByUsername(string username)
        {
            var chessGames = _chessGameRepository.GetWhiteChessGamesByUsername(username);
            List<ChessGameResponse> response = new List<ChessGameResponse>();
            foreach (var chessGame in chessGames)
            {
                var chessGameResponse = new ChessGameResponse(chessGame);
                response.Add(chessGameResponse);
            }
            return Ok(response);
        }

        [HttpGet("user/{username}/blackGames")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetBlackChessGamesByUserId(string username)
        {
            var chessGames = _chessGameRepository.GetBlackChessGamesByUsername(username);
            List<ChessGameResponse> response = new List<ChessGameResponse>();
            foreach (var chessGame in chessGames)
            {
                var chessGameResponse = new ChessGameResponse(chessGame);
                response.Add(chessGameResponse);
            }
            return Ok(response);
        }

        [HttpGet("user/{username}/games")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGame>))]
        public IActionResult GetChessGamesByUsername(string username)
        {
            var chessGames = _chessGameRepository.GetChessGamesByUsername(username);
            List<ChessGameResponse> response = new List<ChessGameResponse>();
            foreach (var chessGame in chessGames)
            {
                var chessGameResponse = new ChessGameResponse(chessGame);
                response.Add(chessGameResponse);
            }
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChessGameResponse>))]
        public IActionResult GetChessGameById(int id)
        {
            var chessGame = _chessGameRepository.GetChessGameById(id);
            if (chessGame == null)
            {
                return NotFound();
            }
            ChessGameResponse chessGameResponse = new ChessGameResponse(chessGame);
            return Ok(chessGameResponse);
        }

        [HttpPost("createGame")]
        public IActionResult CreateChessGame(CreateChessGameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User whitePlayer = _userRepository.GetUserByUsername(request.WhitePlayerUsername);
            User blackPlayer = _userRepository.GetUserByUsername(request.BlackPlayerUsername);

            if (whitePlayer == null || blackPlayer == null)
            {
                return BadRequest("A username was not found");
            }

            ChessGame chessGame = new ChessGame(whitePlayer.Id, whitePlayer.Username, blackPlayer.Id, blackPlayer.Username,
                request.DateStarted, request.DateFinished, request.Result, request.StartTime, request.Increment, request.PGN);
            
            if (_chessGameRepository.CreateChessGame(chessGame))
            {
                return Created();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
