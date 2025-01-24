using ChessServer.Models.Entities;

namespace ChessServer.Interfaces
{
    public interface IAuthService
    {
        public String Login(User user);

    }
}
