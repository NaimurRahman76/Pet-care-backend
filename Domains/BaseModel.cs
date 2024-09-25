namespace PetCareBackend.Domains
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime CreatedOnUtc { get; set; } 

        public DateTime UpdatedOnUtc { get; set; } 
    }
}
