using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Requests
{
    public class CreateLiveChessGameRequest
    {
        [Required]
        [RegularExpression("^(White|Black)$", ErrorMessage = "Colour must be either White or Black.")]
        public string Colour {  get; set; }
        [Required]
        public string OpponentUsername { get; set; }
        [Required]
        [Range(1, 120)]
        public int StartTime { get; set; }
        [Required]
        [Range(0, 60)]
        public int Increment { get; set; }
    } 
}
