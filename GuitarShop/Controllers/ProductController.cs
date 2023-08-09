using GuitarShop.Data;
using GuitarShop.Data.DataAccess;
using GuitarShop.Models;
using GuitarShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace GuitarShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public ProductController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public int iPageSize = 4;


        //Sorting
       public IActionResult List(string id = "all", string sortBy = "name", int productPage = 1)
        {
            IEnumerable<Product> products;
            Expression<Func<Product, object>> orderBy;
            string orderByDirection;
            int iTotalProducts;

            ViewData["NameSortParam"] = sortBy == "name" ? "name_desc" : "name";
            ViewData["PriceSortParam"] = sortBy == "price" ? "price_desc" : "price";

            if (sortBy.EndsWith("_desc"))
            {
                sortBy = sortBy.Substring(0, sortBy.Length - 5);
                orderByDirection = "desc";
            }
            else
            {

                orderByDirection = "asc";
            }

            //Route values are set to lower case and does not match actual property name
            //Do the following to convert the first letter to uppercase (name -> Name)
            string sPropertyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sortBy);

            orderBy = p => EF.Property<object>(p, sPropertyName);  //e.g. p => p.Name

            if (id == "all")
            {
                iTotalProducts = _repo.Product.FindAll().Count();
                products = _repo.Product.GetProductsWithOptions(new QueryOptions<Product>
                {
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    PageNumber = productPage,
                    PageSize = iPageSize
                });
            }
            else
            {
                int iCatId = _repo.Category.FindByCondition(c => c.CategoryName.ToLower() == id)
                               .FirstOrDefault().CategoryID;
                iTotalProducts = _repo.Product.FindByCondition(p => p.CategoryID == iCatId)
                                .Count();
                products = _repo.Product.GetProductsWithOptions(new QueryOptions<Product>
                {
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    Where = p => p.CategoryID == iCatId,
                    PageNumber = productPage,
                    PageSize = iPageSize
                }); 
            }

            var model = new ProductListViewModel
            {
                Products = products,
                SelectedCategory = id,
                PagingInfo = new PagingInfoViewModel
                {
                    CurrentPage = productPage,
                    ItemsPerPage = iPageSize,
                    TotalItems = iTotalProducts
                }
            };

            // bind products to view
            return View(model);
        }

        public IActionResult Details(int id)
        {
            Product product = _repo.Product.GetById(id);
            if (product != null)
            {
                // use ViewBag to pass data to view
                ViewBag.ImageFilename = product.Code + "_m.png";

                // bind product to view
                return View(product);
            }
            else
                return RedirectToAction("List");
        }

    }
}
