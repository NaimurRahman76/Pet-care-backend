using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCareBackend.DTOs;
using PetCareBackend.Services;
using System.Security.Claims;

namespace PetCareBackend.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService,
            ITokenService tokenService)
        {
            _userService = userService;
            this._tokenService = tokenService;
        }

        // POST: api/auth/signup
        [HttpPost("api/auth/signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpDto signupDto)
        {
            if (string.IsNullOrWhiteSpace(signupDto.Username) || string.IsNullOrWhiteSpace(signupDto.Password) ||
                string.IsNullOrWhiteSpace(signupDto.Gender) || string.IsNullOrWhiteSpace(signupDto.Email))
                return BadRequest(new { message = "Username and password are required" });

            var result = await _userService.RegisterAsync(signupDto.Email, signupDto.Username, signupDto.Password, signupDto.Gender);
            if (!result)
                return Conflict(new { message = "User already exists" });

            return Ok(new { message = "User registered successfully" });
        }


        // POST: api/auth/login
        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            if (token == null)
                return Unauthorized("Invalid credentials");
            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            // Save refresh token in the database (or some persistence)
            await _userService.SaveRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiryTime);
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,//for development use false
                Expires = DateTime.UtcNow.AddMinutes(15),
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,//for development use false
                Expires = DateTime.UtcNow.AddDays(7), // Longer expiration
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Login successful",name=user.FullName});
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("No refresh token found");

            var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
                return Unauthorized("Invalid refresh token");

            // Check if refresh token has expired
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            // Generate new access token and optionally a new refresh token
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var accessToeknExpiredTimeInUtc = DateTime.UtcNow.AddMinutes(15);
            var refreshToeknExpiredTimeInUtc = DateTime.UtcNow.AddDays(7);
            // Update refresh token in the database
            await _userService.SaveRefreshTokenAsync(user.Id, newRefreshToken, refreshToeknExpiredTimeInUtc);

            // Set new tokens in HttpOnly cookies
            Response.Cookies.Append("access_token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = accessToeknExpiredTimeInUtc,
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = refreshToeknExpiredTimeInUtc, 
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Token refreshed successfully" });
        }

        // POST: api/auth/logout
        [Authorize]
        [HttpPost("api/auth/logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
                return Unauthorized("Invalid refresh token");

            // Check if refresh token has expired
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");
            // Clear the JWT token from the cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None 
            };
            Response.Cookies.Delete("refresh_token",cookieOptions);
            Response.Cookies.Delete("access_token",cookieOptions);
            return Ok(new { message="Logged out successfully" });
        }

        [HttpGet("api/auth/IsAuthenticated")]
        [Authorize]  
        public IActionResult IsAuthenticated()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated."); 
            }
            return Ok(new { isAuthenticated = true });
        }
    }
}
