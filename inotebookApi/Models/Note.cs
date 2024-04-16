using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    }
    public class CreateNoteRequest
    {
        [JsonPropertyName("title")] // Specify the JSON property name
        [Required]
        public string Title { get; set; }

        [JsonPropertyName("description")] // Specify the JSON property name
        [Required]
        public string Description { get; set; }

        [JsonPropertyName("tag")] // Specify the JSON property name
        public string Tag { get; set; }
    }
}
