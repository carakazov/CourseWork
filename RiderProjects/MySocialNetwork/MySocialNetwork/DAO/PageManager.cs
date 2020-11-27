﻿using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MySocialNetwork.DTO;
using MySocialNetwork.Utils;

namespace MySocialNetwork.DAO
{
    public class PageManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        private Mapper mapper = new Mapper();

        public Wall GetWall(int wallId)
        {
            try
            {
                Wall wall = dbContext.Walls.Where(w => w.Id == wallId).Include(w => w.WallType).First();
                IEnumerable<Post> posts = dbContext.Posts.Where(p => p.WallId == wallId).Include(p => p.Author)
                    .Include(p => p.Content);
                wall.Posts = posts.ToList();
                return wall;
            }
            catch
            {
                throw;
            }
        }
    }
}