using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MySocialNetwork.DAO;

namespace MySocialNetwork.DTO
{
    public class FindUsersDto 
    {
        [Required]
        public string FirstName { get; set; }
        [DefaultValue("")]
        public string SecondName { get; set; }
        [DefaultValue("")]
        public string MiddleName { get; set; }
        [DefaultValue(0)]
        [Range(0, 120, ErrorMessage = "Please, input message between 0 and 120")]
        public int MinAge { get; set; }
        [DefaultValue(120)]
        [Range(0, 120, ErrorMessage = "Please, input message between 0 and 120")]
        public int MaxAge { get; set; }
    }
}