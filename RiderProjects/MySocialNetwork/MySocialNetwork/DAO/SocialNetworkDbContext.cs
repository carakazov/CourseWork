﻿using System;
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
            modelBuilder.Entity<User>().HasMany(u => u.FriendshipTypes).WithRequired(fr => fr.TypeOwner);
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Content)
                .WithMany(c => c.Posts)
                .Map(pc =>
                {
                    pc.ToTable("Post_content");
                    pc.MapLeftKey("post_id");
                    pc.MapRightKey("content_id");
                });
            modelBuilder.Entity<User>().HasMany(u => u.Friendships).WithRequired(f => f.User);
            modelBuilder.Entity<User>().HasMany(u => u.FriendshipsInvoke).WithRequired(f => f.Friend);
            modelBuilder.Entity<User>().HasMany(u => u.ScoredPosts);
            modelBuilder.Entity<User>().HasMany(u => u.SentRequests).WithRequired(fr => fr.Sender);
            modelBuilder.Entity<User>().HasMany(u => u.ReceivedRequests).WithRequired(fr => fr.Receiver);
            modelBuilder.Entity<Post>().HasMany(p => p.ScoredPosts);
            modelBuilder.Entity<User>().HasMany(u => u.FirstUserDialogs).WithRequired(d => d.FirstUser);
            modelBuilder.Entity<User>().HasMany(u => u.SecondUserDialogs).WithRequired(d => d.SecondUser);
            modelBuilder.Entity<Wall>().HasOptional(w => w.Dialog).WithRequired(d => d.Wall);
        }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<Wall> Walls { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WallType> WallTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<ScoredPost> ScoredPosts { get; set; }
        public DbSet<FriendshipRequest> FriendshipRequests { get; set; }
        public DbSet<FriendshipType> FriendshipTypes { get; set; }
    }
}