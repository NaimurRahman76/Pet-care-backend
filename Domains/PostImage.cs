using System.Diagnostics.CodeAnalysis;

namespace PetCareBackend.Domains
{
    public class PostImage : BaseModel
    {
        public int PostId { get; set; } 

        public byte[] ImageData { get; set; } 

        public string ImageType { get; set; }

        public Post Post { get; set; }

    }
}
