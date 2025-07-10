
using BMS.BLL.Services;
using BMS.Models.Models;
using BMS_UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BMS_UI.Controllers;

[Route("Library")]
public class LibraryController : Controller
    {
        private readonly IDbServices<Book> _manageBook;

        public LibraryController(IDbServices<Book> idb)
        {
            _manageBook = idb;
        }


        //view all books
        // GET: /Library
    [HttpGet]
    public IActionResult Index()
        {

                var books = _manageBook.ViewAllBooks();
                return View(books);
  
        }




    //view book by id
    //     GET: /Library/{id}
    //[HttpGet("{id}")]
    //public IActionResult GetBook(int id)
    //{

    //        var result = _manageBook.ViewBook(id);
    //        if (result == null)
    //        {
    //            return NotFound($"Book with ID {id} not found");
    //        }
    //        return View(result);
    //    }

    //}

    public IActionResult AddBook ()
    {
        return View();
    }


    [HttpPost]
    public IActionResult AddBook([FromForm] AddBook book)  
    {
        if (book == null)
        {
            ViewBag.BadRequest = "Book data is required";
            return View(book);  
        }

        if (!ModelState.IsValid)
        {
            
            return View(book);
        }

        
        var domainBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            PublishedYear = book.PublishedYear
        };

        var result = _manageBook.AddBook(domainBook);

        if (result <= 0)
        {
            ViewBag.BadRequest = "Book failed to add!";
            return View(book);  
        }

        
      
        TempData["SuccessMessage"] = "Book added successfully!";
        return RedirectToAction("Index");
    }


    [HttpGet("book/{id}")]
    public IActionResult UpdateBook(int id)
    {
        //var book = _manageBook.ViewBook(id);
        //if (book == null)
        //{
        //    return NotFound();
        //}

 

        return View();
    }



    //update book
    // PUT: /Library/
    [HttpPost("book/{id}")]
        public IActionResult UpdateBook([FromForm] UpdateBook book, int id)
        {
        
                if (book == null)
                {
                    ViewBag.BadRequest="Book data is required";
                    return View();
                }

        var domainBook = new Book
        {
            Id=book.Id,
            Title = book.Title,
            Author = book.Author,
            PublishedYear = book.PublishedYear
        };


        var result = _manageBook.UpdateBook(domainBook);

                if (result <= 0)
                {
                    ViewBag.Error = "Book not updated";
                }

                ViewBag.Rowsupdated=$"{result} no of rows updated";
                TempData["SuccessMessage"] = "Book updated successfully!";

                return RedirectToAction("Index");
            
  
        }
    
        //delete book by id button
        // DELETE: Library/{id}
        [HttpDelete("DeleteBook/{id}")]
        public IActionResult DeleteBook(int id)
        {
        
                var result = _manageBook.DeleteBook(id);
        if (result <= 0)
        {
            ViewBag.Error = "Book Deletion Unsuccessful";
            return View();
        }

        ViewBag.Success = $"{result} Book Successfully Deleted!";
        return RedirectToAction("Index");

    }
    }



