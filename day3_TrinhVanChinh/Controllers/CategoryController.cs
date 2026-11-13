using day3_TrinhVanChinh.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using day3_TrinhVanChinh.Data;
using day3_TrinhVanChinh.Models;

namespace day3_TrinhVanChinh.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController
        private ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger) { 
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View(CategoryData.Categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            _logger.LogInformation($"Fetching details for category with ID: {id}");
            var category = CategoryData.Categories.FirstOrDefault(c => c.Id == id); 
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            int newId = CategoryData.Categories.Any() ? CategoryData.Categories.Max(c => c.Id) + 1 : 1;
            category.Id = newId;
            CategoryData.Categories.Add(category);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var category = CategoryData.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            var existingCategory = CategoryData.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            existingCategory.Name = category.Name;
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
           var category = CategoryData.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = CategoryData.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (category.Products.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete a category that has products assigned to it.");
                return View(nameof(Delete), category);
            }

            CategoryData.Categories.Remove(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
