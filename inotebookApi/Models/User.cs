using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace inotebookApi.Models
{
    public class User
    {
        [Key]
        public int _id { get; set; }

        [Required]
        public string? name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string password { get; set; }
        public DateTime? date { get; set; } = DateTime.Now;
    }
    
    public class User_signup
    {
        [Key]
        public int _id { get; set; }

        [Required]
        public string? name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string password { get; set; }
        public DateTime? date { get; set; } = DateTime.Now;

    }
    public class User_login
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string password { get; set; }

    }
}
