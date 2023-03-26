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

        public PagedList<Product> GetProducts(ProductsParameters productsParameters)
        {
            return PagedList<Product>.ToPagedList(Get().OrderBy(c => c.Name),
                productsParameters.PageNumber, productsParameters.PageSize);

        }
    }
}
