using ChessServer.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChessServer.Models.Requests
{
    public class CreateChessGameRequest
    {
        [Required]
        public string WhitePlayerUsername {  get; set; }
        [Required]
        public string BlackPlayerUsername {  get; set; }
        [Required]
        public DateTime DateStarted { get; set; }
        [Required]
        public DateTime DateFinished { get; set; }
        [Required]
        public Result Result { get; set; }
        [Required]
        [Range(1, 120)]
        public int StartTime { get; set; }
        [Required]
        [Range(0, 60)]
        public int Increment { get; set; }
        [Required]
        public string PGN { get; set; }
    }
}
