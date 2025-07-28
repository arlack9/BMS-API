
using BMS.BLL.Services;
using BMS.BLL.Services.DbServices;
using Mapster;
using BMS.Models.Models;
using BMS_API.EventHandlers;
using BMS_API.Dto;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BMS_API.Controllers;



[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IDbServices<Book> _manageBook;
    private readonly LibraryEventHandlers _leh;
   
    

    //Objectives
        //add return Ok event management 

    public LibraryController(IDbServices<Book> idb , LibraryEventHandlers leh)
    {
        _manageBook = idb;
        _leh = leh;
  

        //register events to methods
        _manageBook.BookAddSucceeded += _leh.HandleBookAdditionSuccess;
        _manageBook.BookDeletionSucceeded += _leh.HandleBookDeletionSuccess;
        _manageBook.BookupdationSucceeded += _leh.HandleBookUpdationSuccess;
        _manageBook.ValidationFailed += _leh.HandleValidationFailure;
    }

    //eventless functions


    //view all books
    // GET: api/Library
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        try
        {
            var books = await _manageBook.ViewAllBooks();
            return Ok(books);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    //search books by keyword
    [HttpGet("search/{keywords}")]
    public async Task <IActionResult> SearchBook(string keywords)
    {
        try
        {
           var result= await _manageBook.SearchBooks(keywords);
            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server error{ex}");
        }
    }


    //view book by id
    // GET: api/Library/{id}
    [HttpGet("{id}")]
    public async Task <IActionResult> GetBook(int id)
    {
        try
        {
            var result = await _manageBook.ViewBook(id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }


 
 //event triggered functions

    //add book 
    // POST: api/Library
    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
    {
        try
        {
            if (bookDto == null)
            {
                return  BadRequest("Book data is required");
            }

            //map BookDto to Book
            var book = bookDto.Adapt<Book>();

            //trigger events
            await _manageBook.AddBook(book);

            //event status return
            var result = _leh.tempData["status"];
            Console.WriteLine($"status during post: {result}");

           return Ok(result);


        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }

    //update book
    // PUT: api/Library/
    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] BookDto bookDto)
    {
        try
        {
            if (bookDto == null)
            {
                return BadRequest("Book data is required");
            }


            //map BookDto to Book
            var book = bookDto.Adapt<Book>();
            
            //trigger event
            await _manageBook.UpdateBook(book);

            //return status
            var result = _leh.tempData["status"];
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }
    
    //delete book by id
    // DELETE: api/Library/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            await _manageBook.DeleteBook(id);


            
            //event returning
            var result = _leh.tempData["status"];
            return Ok(result);

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }
}
