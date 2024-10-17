using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCareBackend.Domains;
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
        private readonly IUserService _userService;

        public PostController(IPostService postService,
            IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var posts = await _postService.GetAllPostsAsync(pageNumber, pageSize);
            return Ok(posts);
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] PostCreationDTO postCreationDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated."); // This line is optional
            }
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var post = await _postService.CreatePostAsync(postCreationDto,currentUserId);
            return Ok(post);
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
        [HttpGet("GetJson")]
        public async Task<IActionResult> GetTestJson()
        {
            var user = await _userService.GetUserByIdAsync(11);
            if (user != null)
            {
                return Ok(user);
            }
            return Ok();
        }
        [HttpPost("PostJson")]
        public async Task<IActionResult> PostTestJson([FromBody] User user)
        {
            if (user != null)
            {
                user = await _userService.UpdateUserAsync(user);
                return Ok(user);
            }
            return Ok();
        }
    }

}
