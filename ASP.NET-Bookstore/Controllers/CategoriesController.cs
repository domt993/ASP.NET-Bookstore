using Microsoft.AspNetCore.Mvc;
using ASP.NET_Bookstore.Models;
using System.ComponentModel;

namespace ASP.NET_Bookstore.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            // Use the category model to generate 10 categories in memory for display in the view
            var categories = new List<Category>();
            for (int i = 1; i <= 10; i++)
            {
                categories.Add(new Category
                {
                    CategoryId = i,
                    Name = "Category " + i
                });
            }
            return View(categories);
        }
        public IActionResult Browse(string category)
        {
            // Display the selected category using ViewBag object
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.category = category;
            return View();
        }
        public IActionResult Create()
        {
            // display the create category form
            return View();
        }
    }
}
