using System;
using System.Collections.Generic;

namespace MySocialNetwork.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public DateTime PostingDate { get; set; }
        public AuthorDto Author { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        
        public int WallId { get; set; }
        
        public List<byte[]> Photos { get; set; }
        public List<byte[]> Videos { get; set; }
        public List<byte[]> Sounds { get; set; }

        public PostDto()
        {
            Photos = new List<byte[]>();
            Videos = new List<byte[]>();
            Sounds = new List<byte[]>();
        }
    }
}