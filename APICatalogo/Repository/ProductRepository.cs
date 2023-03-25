using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogoAPIContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetOrderProductsByPrice()
        {
            return Get().OrderBy(c => c.Price).ToList();
        }

        public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
        {
            return Get().OrderBy(on => on.Name)
                .Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
                .Take(productsParameters.PageSize).ToList();

        }
    }
}
