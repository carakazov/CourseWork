using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialNetwork.DAO
{
    [Table("Dialogs")]
    public class Dialog
    {
        [ForeignKey("Wall")]
        public int Id { get; set; }
        [Column("first_user_id")]
        public int FirstUserId { get; set; }
        [Column("second_user_id")]
        public int SecondUserId { get; set; }
        public bool Unread { get; set; }
        
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
        public Wall Wall { get; set; }
    }
    
}