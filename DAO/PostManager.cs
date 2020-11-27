using System;
using System.CodeDom;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using MySocialNetwork.DTO;
using MySocialNetwork.Models.SocialNetwork;

namespace MySocialNetwork.DAO
{
    public class PostManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        public void AddNewPost(Post post)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Posts.Add(post);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void ScorePost(int postId, ScoreTypes types)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Post post = dbContext.Posts.Where(p => p.Id == postId).First();
                    switch (types)
                    {
                        case ScoreTypes.Positive:
                            post.Rating += 1;
                            break;
                        case ScoreTypes.Negative:
                            post.Rating -= 1;
                            break;
                    }
                    
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void AddScoredPost(ScoredPost scoredPost)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.ScoredPosts.Add(scoredPost);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}