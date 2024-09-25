using PetCareBackend.Domains;
using PetCareBackend.DTOs;
using PetCareBackend.Models;

namespace PetCareBackend.Services
{
    public interface IPostService
    {
        Task<List<PostDetailsDTO>> GetAllPostsAsync(int pageNumber, int pageSize);
        Task<Post> CreatePostAsync(PostCreationDTO postCreationDto, int userId);
        Task<Post> UpdatePostAsync(int postId, PostCreationDTO postUpdateDto,int userId);
        Task<bool> DeletePostAsync(int postId,int userId);
    }
}
