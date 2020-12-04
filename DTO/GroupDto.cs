using System;
using System.Collections.Generic;

namespace MySocialNetwork.DTO
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string GroupType { get; set; }
        public int Rating { get; set; }
        
        public List<WallDto> Walls { get; set; }
        public List<ReaderProfileDto> ReaderProfiles { get; set; }

        public GroupDto()
        {
            Walls = new List<WallDto>();
            ReaderProfiles = new List<ReaderProfileDto>();

        }
    }
}