using System.ComponentModel.DataAnnotations;

namespace SiiTaxi.Models
{
    public class People
    {
        [Key]
        public int PeopleID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OtherEmail { get; set; }
    }
}