using MoviesServices.Models;
using System.Threading.Tasks;

namespace MoviesServices.Services
{
    public interface IUserRepository
    {
        Task<int> AddUser(Users user);
        Task<int> DeleteUser(int? userID);
        Task<int> UpdateUser(Users user);
    }
}
