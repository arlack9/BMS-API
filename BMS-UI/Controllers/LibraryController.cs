using AutoMapper;
using BMS.BLL.Services.DbServices;
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

    public LibraryController(IDbServices<Book> idb, UserManager<IdentityUser> userManager, LibraryEventHandlers leh)
    {
            _manageBook = idb;     
            _userManager = userManager;
            _leh = leh;


        //events manage 
            _manageBook.BookoperationSucceeded += _leh.HandleBookOperationSuccess;
            _manageBook.BookDeletionSucceeded += _leh.HandleBookDeletionSuccess;
            _manageBook.ValidationFailed += _leh.HandleValidationFailure;
    }

    // GET: /Library
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string keywords)
    {

        if (keywords != null)
        {
            var SearchedBooks = await _manageBook.BookSearch(keywords);
            return View(SearchedBooks);
        }

        var books = await _manageBook.ViewAllBooks();
        return View(books);


    }

    // AddBook - GET
    [HttpGet("book/")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> AddBook()
    {
        return View();
    }

    // AddBook - POST
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

        
        var domainBook = book.Adapt<Book>();
        await _manageBook.AddBook(domainBook);
        
        return RedirectToAction("Index");
    }

    // UpdateBook - GET
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

        var updateBookViewModel = book.Adapt<UpdateBook>();
        return View(updateBookViewModel);
    }

    // UpdateBook - POST
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

     
        book.Id = id;

       
        var domainBook = book.Adapt<Book>();
        await _manageBook.UpdateBook(domainBook);
        
        return RedirectToAction("Index");
    }

    // DeleteBook
    [HttpPost("DeleteBook/{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> DeleteBook(int id)
    {
       
        await _manageBook.DeleteBook(id);
        
        return Ok(new { success = true, message = "Operation completed" });
    }
}



