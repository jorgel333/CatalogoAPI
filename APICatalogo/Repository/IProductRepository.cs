using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
        IEnumerable<Product> GetOrderProductsByPrice();
    }
}
