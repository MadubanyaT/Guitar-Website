namespace GuitarShop.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppDbContext _appDbContext;
        private IProductRepository _product;
        private ICategoryRepository _category;

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_appDbContext);
                }
                return _product;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
				if (_category == null)
                {
                    _category = new CategoryRepository(_appDbContext);
                }
                return _category;
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
