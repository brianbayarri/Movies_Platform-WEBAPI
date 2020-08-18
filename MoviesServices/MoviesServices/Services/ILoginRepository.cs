using MoviesServices.Models;
using System.Threading.Tasks;

namespace MoviesServices.Services
{
    public interface ILoginRepository
    {
        Task<Users> AuthenticateUser(string userEmail, string password);
        Task<string> GetRole(Users users);
        Task<int> AddUser(Users user);
        string GenerateJSONWebToken(string name, string userEmail, string role);
    }
}
