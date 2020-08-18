using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesServices.Models
{
    public partial class Users
    {
        public Users()
        {
            Category = new HashSet<Category>();
        }

        [JsonIgnore]
        public int UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public int UserTypeId { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }

        [JsonIgnore]
        public virtual ICollection<Category> Category { get; set; }
    }
}
