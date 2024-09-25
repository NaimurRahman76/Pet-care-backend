namespace PetCareBackend.DTOs
{
    public class PostDetailsDTO
    {
        public int PostId { get; set; }
        public string Body { get; set; }
        public string? Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
