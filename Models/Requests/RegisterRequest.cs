using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(20)]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string Username { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
