using AutoMapper;
using BMS.BLL.Services;
using BMS.BLL.Services.EventHandlers;
using BMS.Models.Models;
using BMS_UI.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BMS_UI.Controllers;

[Authorize]
[Route("Library")]
public class LibraryController : Controller
{
    private readonly IDbServices<Book> _manageBook;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly LibraryEventHandlers _leh;


    public LibraryController(IDbServices<Book> idb , UserManager<IdentityUser> userManager, LibraryEventHandlers leh)
    {
        _manageBook = idb;     
        _userManager = userManager;
        _leh = leh;

        //register events
        
        
        _manageBook.BookoperationSucceeded += _leh.HandleSuccess;
        _manageBook.ValidationFailed += _leh.HandleValidationFailure;
        _manageBook.BookDeletionSucceeded += _leh.HandleFailure;
    }


    //view all books
    // GET: /Library
    [HttpGet]
    [AllowAnonymous]
    
    public async Task<IActionResult> Index()
    {
        var books = await _manageBook.ViewAllBooks();
        return View(books);
    }



    //AddBook
    [HttpGet("book/")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> AddBook ()
    {
        Console.WriteLine("just view");
        return View();
    }

    [HttpPost("book/")]
    [Authorize(Roles ="Admin")]
    
    public async Task<IActionResult> AddBook([FromForm] AddBook book)  
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



        //Mapster
        var domainBook = book.Adapt<Book>();
        Console.WriteLine(domainBook);

        await _manageBook.AddBook(domainBook);

 
        TempData["SuccessMessage"] = "Book added successfully!";
        return RedirectToAction("Index");
    }



    //UpdateBook
    [HttpGet("book/{id}")]
    [Authorize(Roles ="Admin")]
    
    public async Task<IActionResult> UpdateBook(int id)
    {
        var book = await _manageBook.ViewBook(id);
        if (book == null)
        {
            ViewBag.BadRequest = "Book doesnt exist!";
            return RedirectToAction("Index");
        }

        // Map the Book to UpdateBook ViewModel
        var updateBookViewModel = book.Adapt<UpdateBook>();
        return View(updateBookViewModel);
    }


    // POST: /Library/ form only support post , not put 
    [HttpPost("book/{id}")]
    [Authorize(Roles ="Admin")]
    
    public async Task<IActionResult> UpdateBook([FromForm] UpdateBook book, int id)
    {
    
        if (book == null)
        {
           ViewBag.BadRequest="Book data is required";
           return View();
        }

        if (!ModelState.IsValid)
        {
            return View(book);
        }

        // Ensure the ID from the route is set on the book object
        book.Id = id;

        //Mapster DTO-> Book
        var domainBook = book.Adapt<Book>();

        await _manageBook.UpdateBook(domainBook);

        TempData["SuccessMessage"] = "Book updated successfully!";
        return RedirectToAction("Index");
    }


    //delete book by id button
    // DELETE: Library/{id}
    [HttpPost("DeleteBook/{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles ="Admin")]
    
    public async Task<IActionResult> DeleteBook(int id)
    {
       await _manageBook.DeleteBook(id);


        ViewBag.Success = $" Book with Id {id} Successfully Deleted!";
     

        return Ok(new { success = true, message = $"Book with ID {id} deleted." });

    }



}



