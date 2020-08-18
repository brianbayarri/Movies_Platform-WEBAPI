using System.ComponentModel.DataAnnotations;

namespace MoviesServices.ViewModel
{
    public class CategoryViewModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
