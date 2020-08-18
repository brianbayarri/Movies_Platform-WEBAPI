using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesServices.Models
{
    public partial class Movie
    {
        [JsonIgnore]
        public int MovieId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
