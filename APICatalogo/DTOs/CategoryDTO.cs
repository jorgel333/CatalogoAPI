using APICatalogo.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class CategoryDTO
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }
        public ICollection<ProductDTO>? Products { get; set; }

        public CategoryDTO()
        {
            Products = new Collection<ProductDTO>();
        }
    }
}
