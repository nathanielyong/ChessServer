using ChessServer.Models.Entities;

namespace ChessServer.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        bool CreateUser(User user);
        bool Save();
    }
}
