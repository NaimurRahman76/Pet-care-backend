
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
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), 
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

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Email = email,
                Password = hashedPassword,
                Role = Roles.User.ToString() ,
                FullName = username,
                Gender = gender,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
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

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _applicationDbContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            var user = await _applicationDbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            var user = _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _applicationDbContext.Users.Update(user);
            await _applicationDbContext.SaveChangesAsync();
            return user;
        }

        #endregion
    }
}
