namespace PetCareBackend.DTOs
{
    public class PostListResponseDTO
    {
        public List<PostDetailsDTO> Posts { get; set; }
        public bool HasMore { get; set; }
    }
}
