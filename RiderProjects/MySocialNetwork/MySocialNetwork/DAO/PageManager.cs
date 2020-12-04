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

        public Wall CreateWall(WallTypes type, string title)
        {
            WallType wallType = dbContext.WallTypes.Where(wt => wt.Title == type.ToString()).First();
            Wall wall = new Wall()
            {
                Title = title,
                WallType = wallType
            };
            wall = dbContext.Walls.Add(wall);
            dbContext.SaveChanges();
            return wall;
        }
    }
}