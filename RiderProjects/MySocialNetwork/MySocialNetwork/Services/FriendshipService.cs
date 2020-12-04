using System;
using System.Collections.Generic;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;

namespace MySocialNetwork.Services
{
    public class FriendshipService
    {
        private FriendshipManager friendshipManager = new FriendshipManager();
        private UserManager userManager = new UserManager();
        public void SendRequest(int senderId, int receiverId, DateTime sendingDate)
        {
            FriendshipRequest request = new FriendshipRequest()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                SendingDate = sendingDate
            };
            friendshipManager.AddRequest(request);
        }

        public void DeleteRequest(int senderId, int receiverId)
        {
            friendshipManager.DeleteRequest(senderId, receiverId);
        }

        public void StartNewFriendship(int userId, int friendId)
        {
            //FriendshipType commonType = friendshipManager.FindFriendshipType(userId, "common");
            friendshipManager.StartFriendship(userId, friendId);
        }
    }
}