using ChessServer.Models.Requests;
using ChessServer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ChessServer.Interfaces;

namespace ChessServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        public AuthController(IUserRepository userRepository, IAuthService authService)
        { 
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(errors);
            }
            if (_userRepository.GetUserByUsername(request.Username) != null)
            {
                return BadRequest("Username already taken");
            }
            if (_userRepository.GetUserByEmail(request.Email) != null)
            {
                return BadRequest("Email already in use");
            }
            User user = new User(request.Username, request.Email, request.Password);
            if (_userRepository.CreateUser(user))
            {
                return Created();
            } else
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage)
                              .ToList();
                return BadRequest(errors);
            }
            User user = _userRepository.GetUserByUsername(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                return BadRequest("Username or password incorrect");
            }

            string token = _authService.Login(user);
            return Ok(token);
        }
    }
}
