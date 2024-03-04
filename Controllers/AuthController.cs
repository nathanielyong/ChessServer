using ChessServer.Models.Requests;
using ChessServer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ChessServer.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ChessServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new User(request.Username, request.Email, hashedPassword);
            if (_userRepository.CreateUser(user))
            {
                return Ok(user);
            } else
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage)
                              .ToList();
                return BadRequest(errors);
            }
            User user = _userRepository.GetUserByUsername(request.Username);
            if (user == null)
            {
                return BadRequest("Username not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                return BadRequest("The password you entered is incorrect");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
