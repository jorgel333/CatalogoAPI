using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} e no minímo {2} caracteres",
            MinimumLength = 5)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 8)]
        public string? ImageUrl { get; set; }
        public ICollection<Product>? Products { get; set; }

        public Category()
        {
            Products = new Collection<Product>();
        }
    }
}
