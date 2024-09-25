
using Microsoft.EntityFrameworkCore;
using PetCareBackend.Domains;
using PetCareBackend.DTOs;
using PetCareBackend.Models;

namespace PetCareBackend.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PostService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<PostDetailsDTO>> GetAllPostsAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Posts.CountAsync(); 

            var posts = await _context.Posts
                .Include(p => p.PostImages)
                .OrderByDescending(p => p.CreatedOnUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var postDetails = posts.Select(post => new PostDetailsDTO
            {
                PostId = post.Id,
                Body = post.Body,
                Username = post.UserName,
                CreatedAt = post.CreatedOnUtc,
                ImageUrls = post.PostImages.Select(pi => Convert.ToBase64String(pi.ImageData)).ToList()
            }).ToList();

            return postDetails;
        }

        public async Task<Post> CreatePostAsync(PostCreationDTO postCreationDto, int userId)
        {
            var post = new Post
            {
                UserId = userId,
                Body = postCreationDto.Body,
                UserName = postCreationDto.Username,
                CreatedOnUtc = DateTime.UtcNow
            };

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Save images if any
            if (postCreationDto.Images != null && postCreationDto.Images.Any())
            {
                foreach (var image in postCreationDto.Images)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        var postImage = new PostImage
                        {
                            PostId = post.Id,
                            ImageData = memoryStream.ToArray(),
                            ImageType = image.ContentType
                        };

                        await _context.PostImages.AddAsync(postImage);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return post;
        }

        public async Task<Post> UpdatePostAsync(int postId, PostCreationDTO postUpdateDto,int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId && x.UserId ==userId);
            if (post == null) throw new Exception("Post not found");

            post.Body = postUpdateDto.Body;
            post.UserName = postUpdateDto.Username; 

            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<bool> DeletePostAsync(int postId,int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);
            if (post == null) return false;

            // Remove associated images
            _context.PostImages.RemoveRange(post.PostImages);

            // Remove the post itself
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
