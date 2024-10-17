using Microsoft.EntityFrameworkCore;
using PetCareBackend.Domains;

namespace PetCareBackend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostImage>()
                .HasOne(pi => pi.Post) 
                .WithMany(p => p.PostImages) 
                .HasForeignKey(pi => pi.PostId) 
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.RefreshToken)
                .HasMaxLength(256); 

            modelBuilder.Entity<User>()
                .Property(u => u.RefreshTokenExpiryTime);
        }
    }
}
