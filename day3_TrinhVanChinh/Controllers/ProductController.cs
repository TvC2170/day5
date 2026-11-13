using day3_TrinhVanChinh.Data;
using day3_TrinhVanChinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace day3_TrinhVanChinh.Controllers
{
    public class ProductController : Controller
    {
        private static readonly List<Product> products = InitializeProducts();

        private static List<Product> InitializeProducts()
        {
            var initialProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, Description = "Sample product", CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, Description = "Sample product", CategoryId = 2 }
            };

            foreach (var product in initialProducts)
            {
                product.Category = CategoryData.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                product.Category?.Products.Add(product);
            }

            return initialProducts;
        }

        private void PopulateCategories(int? selectedCategoryId = null)
        {
            ViewBag.Categories = new SelectList(
                CategoryData.Categories,
                nameof(Category.Id),
                nameof(Category.Name),
                selectedCategoryId);
        }

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            var category = CategoryData.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(Product.CategoryId), "Please select a valid category.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories(product.CategoryId);
                return View(product);
            }

            product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            product.Category = category;
            products.Add(product);
            category!.Products.Add(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            PopulateCategories(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var category = CategoryData.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(Product.CategoryId), "Please select a valid category.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories(product.CategoryId);
                return View(product);
            }

            existingProduct.Category?.Products.Remove(existingProduct);

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Category = category;
            category!.Products.Add(existingProduct);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Category?.Products.Remove(product);
            products.Remove(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
