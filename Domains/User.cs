using System.ComponentModel.DataAnnotations;

namespace PetCareBackend.Domains
{
    public class User :BaseModel
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public bool IsVerified { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
