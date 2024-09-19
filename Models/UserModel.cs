namespace PetCareBackend.Models
{
    public class UserModel :BaseModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsVerified { get; set; }
    }
}
