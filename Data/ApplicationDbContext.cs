using Microsoft.EntityFrameworkCore;

namespace PetCareBackend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<PetModel> Pets { get; set; }
    }
}
