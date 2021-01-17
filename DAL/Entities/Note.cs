using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Note
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public long user_id { get; set; }
    }
}
