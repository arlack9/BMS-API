
using AutoMapper;
using BMS.BLL.Services;
using BMS.Models.Models;
using BMS_UI.ViewModels;
using Mapster;
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



    //AddBook
    [HttpGet("book/")]
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

        //Automapper
        //var domainBook = _mapper.Map<Book>(book);

        //Mapster
        var domainBook = book.Adapt<Book>();

        

        var result = _manageBook.AddBook(domainBook);

        if (result <= 0)
        {
            ViewBag.BadRequest = "Book failed to add!";
            return View(book);  
        }
 
        TempData["SuccessMessage"] = "Book added successfully!";
        return RedirectToAction("Index");
    }



    //UpdateBook

    [HttpGet("book/{id}")]
    public IActionResult UpdateBook(int id)
    {
        var book = _manageBook.ViewBook(id);
        if (book == null)
        {
            ViewBag.BadRequest = "Book doesnt exist!";
        }


        return View();
    }


    // POST: /Library/ form only support post , not put 
    [HttpPost("book/{id}")]
    public IActionResult UpdateBook([FromForm] UpdateBook book, int id)
    {
    
        if (book == null)
        {
           ViewBag.BadRequest="Book data is required";
           return View();
        }

        //Automapper
        //var domainBook = _mapper.Map<Book>(book);

        //Mapster DTO-> Book
        var domainBook = book.Adapt<Book>();

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
    [HttpDelete("book/{id}")]
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



