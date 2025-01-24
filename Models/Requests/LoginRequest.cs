using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

}
