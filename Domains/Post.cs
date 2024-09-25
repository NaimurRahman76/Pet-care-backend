using System.Diagnostics.CodeAnalysis;

namespace PetCareBackend.Domains
{
    public class Post : BaseModel
    {
        [AllowNull]
        public string UserName { get; set; }

        public string Body { get; set; }

        public int UserId { get; set; }

        public ICollection<PostImage> PostImages { get; set; }

    }
}
