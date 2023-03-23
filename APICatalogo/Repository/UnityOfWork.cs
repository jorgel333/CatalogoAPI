using APICatalogo.Context;

namespace APICatalogo.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private ProductRepository? _productRepository;
        private CategoryRepository? _categoryRepository;
        public CatalogoAPIContext _context;

        public UnityOfWork(CatalogoAPIContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository = _productRepository ?? new ProductRepository(_context);
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);   
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
