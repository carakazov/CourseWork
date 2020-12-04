using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using MySocialNetwork.DTO;

namespace MySocialNetwork.DAO
{
    public class FriendshipManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        public void AddRequest(FriendshipRequest request)
        {
            dbContext.FriendshipRequests.Add(request);
            dbContext.SaveChanges();
        }

        public void DeleteRequest(int senderId, int receiverId)
        {
            FriendshipRequest request = dbContext.FriendshipRequests
                .Where(r => r.SenderId == senderId && r.ReceiverId == receiverId).First();
            dbContext.FriendshipRequests.Remove(request);
            dbContext.SaveChanges();
        }

        public List<FriendshipRequest> GetSentRequestsOfUser(int senderId)
        {
            List<FriendshipRequest> requests = dbContext.FriendshipRequests.Where(fr => fr.SenderId == senderId)
                .Include(fr => fr.Receiver).ToList();
            return requests;
        }

        public List<FriendshipRequest> GetReceivedRequestsOfUser(int receiverId)
        {
            List<FriendshipRequest> requests =
                dbContext.FriendshipRequests.Where(fr => fr.ReceiverId == receiverId).Include(fr => fr.Sender).ToList();
            return requests;
        }

        public void AddFriendshipType(string typeTitle, int ownerId)
        {
            FriendshipType type = new FriendshipType()
            {
                Title = typeTitle,
                TypeOwnerId = ownerId
            };
            dbContext.FriendshipTypes.Add(type);
            dbContext.SaveChanges();
        }
        
        
        
        public void StartFriendship(int userId, int friendId)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Friendship friendship = new Friendship()
                    {
                        UserId = userId,
                        FriendId = friendId,
                        TypeId = null
                    };
                    dbContext.Friendships.Add(friendship);
                    dbContext.SaveChanges();
                    Friendship reverseFriendship = ReverseFriendship(friendship);
                    dbContext.Friendships.Add(reverseFriendship);
                    dbContext.SaveChanges();
                    DeleteRequest(friendId, userId);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private Friendship ReverseFriendship(Friendship friendship)
        {
            //FriendshipType commonType = FindFriendshipType(friendship.UserId, "common");
            Friendship reverseFriendship = new Friendship()
            {
                UserId = friendship.FriendId,
                FriendId = friendship.UserId,
                TypeId = null
            };
            return reverseFriendship;
        }

        public FriendshipType FindFriendshipType(int commonOwnerId, string title)
        {
            FriendshipType type = dbContext.FriendshipTypes.Where(ft => ft.TypeOwnerId == commonOwnerId && ft.Title == title)
                .First();
            return type;
        }
    }
}