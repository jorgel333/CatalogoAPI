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

        public IEnumerable<Category> GetCategoryProduct()
        {
            return Get().Include(p => p.Products);
        }

        public PagedList<Category> GetCategorys(CategorysParameters categorysParameters)
        {
            return PagedList<Category>.ToPagedList(Get().OrderBy(c => c.Name), 
                categorysParameters.PageNumber, categorysParameters.PageSize);
        }
    }
}
