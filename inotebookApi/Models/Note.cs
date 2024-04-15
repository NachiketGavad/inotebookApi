using System.ComponentModel.DataAnnotations;

namespace inotebookApi.Models
{
    public class Note
    {
        [Key]
        public int _id { get; set; }

        [Required]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        public string tag { get; set; }
        public DateTime date { get; set; } = DateTime.Now;

        // other properties
        public int UserId { get; set; } // Foreign key property
        public User User { get; set; } // Navigation property
    }
}
