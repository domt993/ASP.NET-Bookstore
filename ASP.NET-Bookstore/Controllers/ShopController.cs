using ASP.NET_Bookstore.Data;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Bookstore.Controllers
{
    public class ShopController : Controller
    {
        
        // class level dbContext conntection object
        // This is used to access the database
        private readonly ApplicationDbContext _context;
        // This controller is responsible for handling requests to the shop
        // Constructor that acceps a DbContext instance
        // This allows dependency injection to provide the context
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Query that lists the categories from the database in a-z order for display
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }

        public IActionResult ShopByCategory(int id)
        {
            // Query that lists the books in the selected category, ordered by author and title
            var books = _context.Books
                .Where(b => b.CategoryId == id)
                .OrderBy(b => b.Author)
                .ThenBy(b => b.Title)
                .ToList();
            return View(books);
        }
    }
}
