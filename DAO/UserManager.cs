using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MySocialNetwork.DTO;

 namespace MySocialNetwork.DAO
{
    public class UserManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();

        public User GetUserByLogin(string login)
        {
            try
            {
                User user = dbContext.Users.Where(u => u.Login == login).Include(u => u.Walls).Include(u => u.ScoredPosts).First();
                foreach (Wall wall in user.Walls)
                {
                    WallType wallType = dbContext.WallTypes.Where(wt => wt.Id == wall.WallTypeId).First();
                    wall.WallType = wallType;
                }
                return user;
            }
            catch
            {
                throw;
            }
        }

        public void AddUser(User user)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    User addedUser = dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    int mainWallTypeId = dbContext.WallTypes.Where(wt => wt.Title == WallTypes.Main.ToString()).First()
                        .Id;
                    int photoWallTypeId = dbContext.WallTypes.Where(wt => wt.Title == WallTypes.Photos.ToString())
                        .First().Id;
                    Wall mainWall = new Wall()
                    {
                        OwnerId = addedUser.Id,
                        WallTypeId = mainWallTypeId,
                        Title = WallTypes.Main.ToString()
                    };
                    Wall avatarsWall = new Wall()
                    {
                        OwnerId =  addedUser.Id,
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


        public List<User> FindUsers(FindUsersDto userInfo)
        {
            List<User> foundedUsers = dbContext.Users.ToList();
           if (userInfo.FirstName != null)
            {
                foundedUsers = foundedUsers.Where(u => u.FirstName == "Ivan").ToList();
            }

            if (userInfo.SecondName != null)
            {
                foundedUsers = foundedUsers.Where(u => u.SecondName == userInfo.SecondName).ToList();
            }

            if (userInfo.MiddleName != null)
            {
                foundedUsers = foundedUsers.Where(u => u.MiddleName == userInfo.MiddleName).ToList();
            }
            /*
            DateTime now = DateTime.Now;
            foundedUsers = foundedUsers.Where(u =>
                CalculateAge(u.BirthDate, now) <= userInfo.MaxAge && CalculateAge(u.BirthDate, now) >= userInfo.MinAge);*/
            return foundedUsers;
        }

        private int CalculateAge(DateTime birthday, DateTime now)
        {
            TimeSpan ageTimeSpan = now - birthday;
            int age = ageTimeSpan.Days / 365;
            return age;
        }
    }
}