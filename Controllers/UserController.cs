using ChessServer.Interfaces;
using ChessServer.Models.Entities;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("getUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("/username/{username}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("/email/{email}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
