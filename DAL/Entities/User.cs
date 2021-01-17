using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        public long id { get; set; }
        public string username { get; set; }
    }
}
