using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesServices.Models
{
    public partial class Category
    {
        public Category()
        {
            Movie = new HashSet<Movie>();
        }

        [JsonIgnore]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [JsonIgnore]
        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Users User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Movie> Movie { get; set; }
    }
}
