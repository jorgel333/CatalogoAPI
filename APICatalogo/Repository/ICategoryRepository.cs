using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        PagedList<Category> GetCategorys(CategorysParameters categorysParameters);
        IEnumerable<Category> GetCategoryProduct();
        
    }
}
