using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
    }
}
