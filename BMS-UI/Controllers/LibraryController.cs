using BMS.BLL.Services.DbServices;
using BMS.BLL.Services.Events;
using BMS.Models.Models;
using BMS_UI.Dto;
using BMS_UI.EventHandlers;
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

    private readonly IEvents _events;

    public LibraryController(
        IDbServices<Book> idb, 
        UserManager<IdentityUser> userManager, 
        LibraryEventHandlers leh,
        
        IEvents events)
    {
            _manageBook = idb;     
            _userManager = userManager;
            _leh = leh;
            
            //events
            _events = events;

        //events manage 
        _events.BookoperationSucceeded += _leh.HandleBookOperationSuccess;
        _events.BookDeletionSucceeded += _leh.HandleBookDeletionSuccess;
        _events.ValidationFailed += _leh.HandleValidationFailure;
        _events.BookupdationSucceeded += _leh.HandleBookUpdationSuccess;
        _events.BookAddSucceeded += _leh.HandleBookAdditionSuccess;
    }

    // GET: /Library
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string keywords)
    {
        
        if (keywords != null)
        {
            var SearchedBooks = await _manageBook.SearchBooks(keywords);
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

        var updateBookViewModel = book.Adapt<BookDto>();
        return View(updateBookViewModel);
    }



    // UpdateBook - POST
    [HttpPost("book/{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateBook([FromForm] BookDto bookDto)
    {
        if (bookDto == null)
        {
           ViewBag.BadRequest="Book data is required";
           return View();
        }

        if (!ModelState.IsValid)
        {
            return View(bookDto);
        }

     
        //bookDto.Id = id;

       
        var domainBook = bookDto.Adapt<Book>();
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

        return RedirectToAction("Index");
    }
}



