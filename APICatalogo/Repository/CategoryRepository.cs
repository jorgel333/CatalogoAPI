using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogoAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetCategoryProduct()
        {
            return await Get().Include(p => p.Products).ToListAsync();
        }

        public async Task<PagedList<Category>> GetCategorys(CategorysParameters categorysParameters)
        {
            return await PagedList<Category>.ToPagedList(Get().OrderBy(c => c.Name), 
                categorysParameters.PageNumber, categorysParameters.PageSize);
        }
    }
}
