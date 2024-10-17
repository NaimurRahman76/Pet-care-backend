using PetCareBackend.Domains;
using PetCareBackend.Models;

namespace PetCareBackend.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string email,string username, string password,string gender);

        Task<string> AuthenticateAsync(string email, string password);

        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User> GetUserByIdAsync(int id);
        Task<User> UpdateUserAsync(User user);

        Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime refreshTokenExpiryTime);

    }
}
