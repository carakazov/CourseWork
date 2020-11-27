using System.Collections.Generic;
using System.IO;
using MySocialNetwork.Models;
using MySocialNetwork.DAO;

namespace MySocialNetwork.DTO
{
    public class FriendshipDto
    {
        public FriendshipTypes FriendshipTypes { get; set; }
        public List<IEnumerable<UserDto>> Friends { get; set; }
    }
}