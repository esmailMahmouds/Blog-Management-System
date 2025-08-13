using BlogApp.Models.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follow>()
            .HasOne(f => f.FollowerUser)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowerUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.FollowingUser)
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.FollowingUserId)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

			modelBuilder.Entity<Country>()
				.HasData(
					new Country { Id = 1, Name = "Egypt" },
					new Country { Id = 2, Name = "USA" },
					new Country { Id = 3, Name = "KSA" },
                    new Country { Id = 4, Name = "China" },
                    new Country { Id = 5, Name = "Italy" },
                    new Country { Id = 6, Name = "UAE" },
                    new Country { Id = 7, Name = "Spain" },
                    new Country { Id = 8, Name = "Canda" },
                    new Country { Id = 9, Name = "Tunisa" },
                    new Country { Id = 10, Name = "Algeria" }

                );
		}
	}
}
