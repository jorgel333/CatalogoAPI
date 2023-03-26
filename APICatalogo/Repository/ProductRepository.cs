using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogoAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetOrderProductsByPrice()
        {
            return await Get().OrderBy(c => c.Price).ToListAsync();
        }

        public async Task<PagedList<Product>> GetProducts(ProductsParameters productsParameters)
        {
            return await PagedList<Product>.ToPagedList(Get().OrderBy(c => c.Name),
                productsParameters.PageNumber, productsParameters.PageSize);

        }
    }
}
