namespace PetCareBackend.Domains
{
    public class BaseModel
    {
        public int Id { get; set; }

        DateTime? CreatedOnUtc { get; set; } 

        DateTime? UpdatedOnUtc { get; set; } 
    }
}
