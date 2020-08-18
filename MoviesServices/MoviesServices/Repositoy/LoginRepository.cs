using MoviesServices.Models;
using MoviesServices.Services;
using MoviesServices.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace MoviesServices.Repositoy
{
    public class LoginRepository : ILoginRepository
    {
        private readonly MovieService_DBContext db;
        private readonly IConfiguration config;

        public LoginRepository(MovieService_DBContext db, IConfiguration config)
        {
            this.db = db;
            this.config = config;
        }

        public async Task<Users> AuthenticateUser(string userEmail, string password)
        {
            string encryptedPass = MD5.calculateMD5(password);
            return await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail) && x.Password.Equals(encryptedPass));
        }

        public async Task<string> GetRole(Users users)
        {
            return await db.UserType.Where(x => x.UserTypeId == users.UserTypeId)
                                    .Select(x => x.Description).FirstOrDefaultAsync();
        }

        public string GenerateJSONWebToken(string name, string userEmail, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Email, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        public async Task<int> AddUser(Users user)
        {
            if (db.Users.Any(t => t.Email == user.Email))
                return Constants.FAILURE;

            string encryptPass = MD5.calculateMD5(user.Password);
            user.Password = encryptPass;
            user.UserTypeId = Constants.VIEWER_TYPE;
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user.UserId;
        }
    }
}
