namespace PetCareBackend.DTOs
{
    public class PostCreationDTO
    {
        public string Body { get; set; }     
        
        public string? Username { get; set; }  

        public List<IFormFile>? Images { get; set; } 
    }

}
