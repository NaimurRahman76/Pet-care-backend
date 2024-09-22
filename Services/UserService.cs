
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCareBackend.Domains;
using PetCareBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetCareBackend.Services
{
    public class UserService : IUserService
    {
        #region ctor

        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;

        public UserService(IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            this._configuration = configuration;
            this._applicationDbContext = applicationDbContext;
        }

        #endregion

        #region Utility

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15), 
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

        #region Methods

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _applicationDbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null; 

            return GenerateJwtToken(user);
        }

        public async Task<bool> RegisterAsync(string email, string username,string password,string gender)
        {
            if (await _applicationDbContext.Users.AnyAsync(u => u.Email == email))
                return false;

            if (await _applicationDbContext.Users.AnyAsync(u => u.Username == username))
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);


            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                Role = Roles.User.ToString() ,
                Username = username,
                Gender = gender
            };

            _applicationDbContext.Users.Add(user);
            await _applicationDbContext.SaveChangesAsync(); 

            return true;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _applicationDbContext.Users
                   .FirstOrDefaultAsync(u => u.Email == email);
        }

        #endregion
    }
}
