using GuitarShop.Data.DataAccess;
using GuitarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GuitarShop.Data
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public IEnumerable<Product> GetAllProductsWithCategoryDetails()
        {
            return _appDbContext.Products
                .Include(m => m.Category);
        }

        public IEnumerable<Product> GetAllProductsInCategory(string category)
        {
            Category cat = _appDbContext.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == category.ToLower());

            return _appDbContext.Products.Where(p => p.CategoryID == cat.CategoryID);
        }

        public IEnumerable<Product> GetProductsWithOptions(QueryOptions<Product> options)
        {
            IQueryable<Product> query = _appDbContext.Products;

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
            {
                if (options.OrderByDirection == "asc")
                    query = query.OrderBy(options.OrderBy);
                else
                    query = query.OrderByDescending(options.OrderBy);
            }

            if(options.HasPaging)
            {
                query = query.Skip((options.PageNumber -1) * options.PageSize).Take(options.PageSize);
            }
            return query.ToList();

        }
    }
}
