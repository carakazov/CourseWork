using System.Collections.Generic;
using System.Linq;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;

namespace MySocialNetwork.Utils
{
    public class FriendshipMapper
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        private Mapper mapper = new Mapper();
        public List<UserInfoDto> GetSendersOfRequests(UserDto userDto)
        {
            List<UserInfoDto> users = new List<UserInfoDto>();
            if (userDto.ReceivedFriendshipRequests.Count > 0)
            {
                foreach (FriendshipRequestDto request in userDto.ReceivedFriendshipRequests)
                {
                    User user = dbContext.Users.Where(u => u.Id == request.SenderId).First();
                    UserInfoDto sender = mapper.FromUserAuthorDto(user);
                    users.Add(sender);
                }
            }

            return users;
        }
    }
}