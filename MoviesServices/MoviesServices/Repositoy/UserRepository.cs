using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesServices.Repositoy
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieService_DBContext db;

        public UserRepository(MovieService_DBContext db)
        {
            this.db = db;
        }
        public async Task<int> AddUser(Users user)
        {
            if (db.Users.Any(t => t.Email == user.Email))
                return Constants.FAILURE;

            string encryptPass = MD5.calculateMD5(user.Password);
            user.Password = encryptPass;
            user.UserTypeId = Constants.ADMIN_TYPE;
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<int> DeleteUser(int? userID)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserId == userID);
            if (user == null)
                return Constants.FAILURE;

            db.Users.Remove(user);
            return await db.SaveChangesAsync();
        }

        public async Task<int> UpdateUser(Users user)
        {
            if (!db.Users.Any(t => t.UserId == user.UserId))
                return Constants.FAILURE;

            string encryptPass = MD5.calculateMD5(user.Password);
            user.Password = encryptPass;
            db.Users.Update(user);
            return await db.SaveChangesAsync();
        }
    }
}
