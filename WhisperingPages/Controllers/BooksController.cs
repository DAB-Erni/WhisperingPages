using Microsoft.AspNetCore.Mvc;
using WhisperingPages.Models;
using WhisperingPages.Services;

namespace WhisperingPages.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var books = context.Books.OrderByDescending(b => b.Id).ToList();
            return View(books);
        }

        //To create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookDto bookdto)
        {
            if (bookdto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(bookdto);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(bookdto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/books/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                bookdto.ImageFile.CopyTo(stream);
            }

            Book book = new Book()
            {
                ImageFileName = newFileName,
                Title = bookdto.Title,
                Author = bookdto.Author,
                Genre = bookdto.Genre,
                Price = bookdto.Price,
                Description = bookdto.Description,
            };

            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Edit(int id)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            var bookDto = new BookDto()
            {
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Price = book.Price,
                Description = book.Description,
            };

            ViewData["BookId"] = book.Id;
            ViewData["ImageFileName"] = book.ImageFileName;

            return View(bookDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, BookDto bookDto)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = book.Id;
                ViewData["ImageFileName"] = book.ImageFileName;
                return View(bookDto);
            }

            // update the image file if we have a new image file
            string newFileName = book.ImageFileName;

            if (bookDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(bookDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/books/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bookDto.ImageFile.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/books/" + book.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            book.ImageFileName = newFileName;
            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Genre = bookDto.Genre;
            book.Price = bookDto.Price;
            book.Description = bookDto.Description;

            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            string imageFullPath = environment.WebRootPath + "/books/" + book.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Books.Remove(book);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Books");
        }
    }
}
