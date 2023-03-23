using APICatalogo.Context;
using APICatalogo.Models;

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
    }
}
