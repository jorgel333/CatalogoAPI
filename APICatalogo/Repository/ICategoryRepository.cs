using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetCategoryProduct();
        
    }
}
