using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MySocialNetwork.DTO;

namespace MySocialNetwork.DAO
{
    public class GroupManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();

        public Group GetGroupByTitle(string title)
        {
            try
            {
                Group group = dbContext.Groups.Where(g => g.Title == title).Include(g => g.Walls).Include(g => g.ScoredPosts).First();
                foreach (Group wall in group.Walls)
                {
                    WallType wallType = dbContext.WallTypes.Where(wt => wt.Id == wall.WallTypeId).First();
                    wall.WallType = wallType;
                }
                return group;
            }
            catch
            {
                throw;
            }
        }

        public void CreationGroup(Group group)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Group createdGroup = dbContext.Groups.Add(group);
                    dbContext.SaveChanges();
                    int mainWallTypeId = dbContext.WallTypes.Where(wt => wt.Title == WallTypes.Main.ToString()).First()
                        .Id;
                    int photoWallTypeId = dbContext.WallTypes.Where(wt => wt.Title == WallTypes.Photos.ToString())
                        .First().Id;
                    Wall mainWall = new Wall()
                    {
                        GroupId = createdGroup.Id,
                        WallTypeId = mainWallTypeId,
                        Title = WallTypes.Main.ToString()
                    };
                    Wall avatarsWall = new Wall()
                    {
                        GroupId = createdGroup.Id,
                        WallTypeId = photoWallTypeId,
                        Title = WallTypes.Photos.ToString()
                    };
                    dbContext.Walls.Add(mainWall);
                    dbContext.Walls.Add(avatarsWall);
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


        public List<Group> FindGroup(FindGroupsDto groupInfo)
        {
            List<Group> foundedGroups = dbContext.Groups.ToList();
            if (groupInfo.Title != null)
            {
                foundedGroups = foundedGroups.Where(g => g.Title == groupInfo.Title).ToList();
            }
            return foundedUsers;
        }

    }
}