using Microsoft.AspNetCore.Mvc;
using PetCareBackend.DTOs;
using PetCareBackend.Services;

namespace PetCareBackend.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
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

            // Set token in HttpOnly cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new { message = "Login successful", token ,name=user.Username});
        }

        // POST: api/auth/logout
        [HttpPost("api/auth/logout")]
        public IActionResult Logout()
        {
            // Clear the JWT token from the cookie
            Response.Cookies.Delete("jwt");
            return Ok(new { message="Logged out successfully" });
        }
    }
}
