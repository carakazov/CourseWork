using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace MySocialNetwork.DAO
{
    public class SocialNetworkDbContext : DbContext
    {
        public SocialNetworkDbContext():base(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=social_network;User Id=vadim;Password=yfgjktjy1813")
        {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>().HasKey(f => new {f.UserId, f.FriendId});
            modelBuilder.Entity<FriendshipRequest>().HasKey(fr => new {fr.SenderId, fr.ReceiverId});
            modelBuilder.Entity<GroupEnteringRequest>().HasKey(ger => new {ger.SenderId, ger.GroupId});
            modelBuilder.Entity<ScoredPost>().HasKey(sp => new {sp.UserId, sp.PostId});
            modelBuilder.Entity<User>().HasMany(u => u.FriendshipTypes);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Content)
                .WithMany(c => c.Posts)
                .Map(pc =>
                {
                    pc.ToTable("Post_content");
                    pc.MapLeftKey("post_id");
                    pc.MapRightKey("content_id");
                });

            modelBuilder.Entity<User>().HasMany(u => u.ScoredPosts);
            modelBuilder.Entity<Post>().HasMany(p => p.ScoredPosts);
        }
        
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<Wall> Walls { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WallType> WallTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<ScoredPost> ScoredPosts { get; set; }
    }
}