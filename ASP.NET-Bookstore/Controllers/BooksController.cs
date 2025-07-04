using ASP.NET_Bookstore.Data;
using ASP.NET_Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Bookstore.Controllers
{
    // If the user is logged in, they can access this controller
    [Authorize(Roles = "Administrator")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Books.Include(b => b.Category).OrderBy(b => b.Author).ThenBy(b => b.Title);
            return View(await applicationDbContext.ToListAsync());
        }
        [AllowAnonymous]
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.OrderBy(c => c.Name), "CategoryId", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Author,Title,Price,MatureContent,CategoryId")] Book book, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                // Check if an image file is provided
                if (image != null && image.Length > 0)
                {
                    // Upload the image and set the Image property
                    book.Image = UploadImage(image);
                }
                else
                {
                    // If no image is provided, set Image to null
                    book.Image = null;
                }
                _context.Add(book);
                // Save the book to the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.OrderBy(c => c.Name), "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Author,Title,Price,MatureContent,CategoryId")] Book book, IFormFile? image, string? CurrentImage)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if an image file is provided
                    if (image != null && image.Length > 0)
                    {
                        // Upload the image and set the Image property
                        book.Image = UploadImage(image);
                    }
                    else
                    {
                        if (CurrentImage != null)
                        {
                            // If no image is provided, keep the current image
                            book.Image = CurrentImage;
                        }
                    }
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        private static string UploadImage(IFormFile image)
        {
            // use globally unique identifier (GUID) to create a unique file name
            //e.g. 123e4567-e89b-12d3-a456-426614174000-book-cover.jpg
            var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
            // define the path dynamically so it runs on any machine
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books", fileName);
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            // return the file name to be stored in the database
            return fileName;
        }
    }
}
