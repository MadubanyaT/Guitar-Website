using GuitarShop.Data.DataAccess;
using GuitarShop.Models;

namespace GuitarShop.Data
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        IEnumerable<Product> GetAllProductsInCategory(string category);
        IEnumerable<Product> GetAllProductsWithCategoryDetails();
        //Sorting
        IEnumerable<Product> GetProductsWithOptions(QueryOptions<Product> options);
        //Sorting
    }
}
