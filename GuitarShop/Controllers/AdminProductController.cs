using GuitarShop.Data;
using GuitarShop.Models;
using GuitarShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuitarShop.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminProductController : Controller
    {
        private IEnumerable<Category> _categories;
        private readonly IRepositoryWrapper _repo;

        public AdminProductController(IRepositoryWrapper repo)
        {
            _repo = repo;
            _categories = _repo.Category.FindAll()
                .OrderBy(c => c.CategoryName);
        }

        public IActionResult List(string id = "all")
        {
            IEnumerable<Product> products;
            if (id == "all")
            {
                products = _repo.Product.FindAll()
                    .OrderBy(p => p.Name);
            }
            else
            {
                products = _repo.Product.GetAllProductsWithCategoryDetails()
                    .Where(p => p.Category.CategoryName.ToLower() == id.ToLower())
                    .OrderBy(p => p.Name);
            }

            var model = new ProductListViewModel
            {
                //Categories = _categories,
                Products = products,
                SelectedCategory = id
            };

            // bind products to view
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            // create new Product object
            Product product = new();

            // add Category object - prevents validation problem
            product.Category = _repo.Category.GetById(1);  

            // use ViewBag to pass action and category data to view
            ViewBag.Action = "Add";
            ViewBag.Categories = _categories;

            // bind product to AddUpdate view
            return View("AddUpdate", product);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            // get Product object for specified primary key
            Product product = _repo.Product.GetById(id);

            // use ViewBag to pass action and category data to view
            ViewBag.Action = "Update";
            ViewBag.Categories = _categories;

            // bind product to AddUpdate view
            return View("AddUpdate", product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0) // new product
                {
                    _repo.Product.Create(product);
                    TempData["Message"] = $"{product.Name} has been added";
                }
                else  // existing product
                {
                    _repo.Product.Update(product);
                    TempData["Message"] = $"{product.Name} has been updated";
                }
                _repo.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Save";
                ViewBag.Categories = _categories;
                return View("AddUpdate", product);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Product product = _repo.Product.GetById(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _repo.Product.Delete(product);
            TempData["Message"] = $"{product.Name} has been deleted";
            _repo.Save();
            return RedirectToAction("List");
        }
    }
}
