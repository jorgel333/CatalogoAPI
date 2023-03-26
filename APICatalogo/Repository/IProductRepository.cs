using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedList<Product>> GetProducts(ProductsParameters productsParameters);
        Task<IEnumerable<Product>> GetOrderProductsByPrice();
    }
}
