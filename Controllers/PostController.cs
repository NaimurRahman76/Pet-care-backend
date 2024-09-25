using Microsoft.AspNetCore.Mvc;
using PetCareBackend.DTOs;
using PetCareBackend.Services;
using System.Security.Claims;

namespace PetCareBackend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var posts = await _postService.GetAllPostsAsync(pageNumber, pageSize);
            return Ok(posts);
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] PostCreationDTO postCreationDto)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var post = await _postService.CreatePostAsync(postCreationDto,currentUserId);
            return CreatedAtAction(nameof(GetAllPosts), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] PostCreationDTO postUpdateDto)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var updatedPost = await _postService.UpdatePostAsync(id, postUpdateDto,currentUserId);
            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _postService.DeletePostAsync(id, currentUserId);
            if (!result) return NotFound("Post not found or you do not have permission to delete this post.");
            return NoContent();
        }
    }

}
