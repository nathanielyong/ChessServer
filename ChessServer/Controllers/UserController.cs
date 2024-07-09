using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChessServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("profile"), Authorize]
        public IActionResult GetProfile()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _userRepository.GetUserByUsername(username);

            var user_info = new
            {
                Username = user.Username,
                Email = user.Email,
                Rating = user.Rating,
                NumGamesPlayed = user.NumGamesPlayed,
                Wins = user.Wins,
                Losses = user.Losses,
                Draws = user.Draws,
                DatedJoined = user.DateJoined,
                LiveChessGameId = user.LiveChessGameId,
                IsPlaying = user.IsPlaying
            };

            return Ok(user_info);
        }

        [HttpGet("getStats/{username}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUserStats(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var stats = new
            {
                Username = user.Username,
                Rating = user.Rating,
                NumGamesPlayed = user.NumGamesPlayed,
                Wins = user.Wins,
                Losses = user.Losses,
                Draws = user.Draws,
                DatedJoined = user.DateJoined,
                LiveChessGameId = user.LiveChessGameId,
                IsPlaying = user.IsPlaying
            };
            return Ok(stats);
        }
    }
}
