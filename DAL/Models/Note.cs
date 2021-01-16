using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Note
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int user_id { get; set; }
    }
}
