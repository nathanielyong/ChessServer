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
    public class LiveChessGameController : Controller
    {
        private readonly ILiveChessGameService _liveChessGameService;
        private readonly IUserRepository _userRepository;
        public LiveChessGameController(ILiveChessGameService liveChessGameService, IUserRepository userRepository)
        {
            _liveChessGameService = liveChessGameService;
            _userRepository = userRepository;
        }

        [HttpGet("getGame/{id:int}")]
        public IActionResult GetLiveGameById(int id) 
        {
            return Ok(_liveChessGameService.GetGameState(id));
        }

        [HttpGet("getCurrentGame/"), Authorize]
        public IActionResult GetCurrentGame()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            return Ok(_liveChessGameService.GetGameState(username));
        }

        [HttpPost("makeMove/"), Authorize]
        public IActionResult MakeMove([FromQuery] string move) 
        { 
            if (move == null)
            {
                return BadRequest(new ErrorResponse("move cannot be null"));
            }
            var username = User.FindFirstValue(ClaimTypes.Name);
            var response = _liveChessGameService.MakeMove(username, move);
            if (response == null)
            {
                return BadRequest("Illegal move");
            }
            return Ok(response);
        }

        [HttpPost("newGame/"), Authorize]
        public IActionResult CreateNewGame(CreateLiveChessGameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User opponent = _userRepository.GetUserByUsername(request.OpponentUsername);
            if (opponent == null) 
            { 
                return BadRequest(new ErrorResponse("Opponent uesrname was not found.")); 
            }
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (request.Colour == "White")
            {
                if (!_liveChessGameService.CreateNewGame(username, request.OpponentUsername, request.StartTime, request.Increment))
                {
                    return BadRequest(new ErrorResponse("A user is already in a game"));
                }
                return Created();
            }
            else 
            {
                if (!_liveChessGameService.CreateNewGame(request.OpponentUsername, username, request.StartTime, request.Increment))
                {
                    return BadRequest(new ErrorResponse(""));
                }
                return Created();
            }
        }
    }
}