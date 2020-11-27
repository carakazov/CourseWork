﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialNetwork.DAO
{
    [Table("Walls")]
    public class Wall
    {
        public int Id { get; set; }
        [Column("wall_type_id")]
        public int WallTypeId { get; set; }
        public string Title { get; set; }
        [Column("owner_id")]
        public int? OwnerId { get; set; }
        [Column("group_id")]
        public int? GroupId { get; set; }
        [Column("dialog_id")]
        public int? DialogId { get; set; }
        
        public Dialog Dialog { get; set; }
        public Group Group { get; set; }
        public User Owner { get; set; }
        public WallType WallType { get; set; }
        public ICollection<Post> Posts { get; set; }

        public Wall()
        {
            Posts = new HashSet<Post>();
        }
    }
}