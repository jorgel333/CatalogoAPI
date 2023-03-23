using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogoAPIContext context) : base(context)
        {
        }

        public IEnumerable<Category> GetCategoryProduct()
        {
            return Get().Include(p => p.Products);
        }
    }
}
