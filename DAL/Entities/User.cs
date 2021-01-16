using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
    }
}
