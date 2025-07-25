
using BMS.BLL.Services;
using BMS.BLL.Services.DbServices;

using BMS.Models.Models;
using BMS_API.EventHandlers;
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
    public ActionResult<IEnumerable<Book>> GetAllBooks()
    {
        try
        {
            var books = _manageBook.ViewAllBooks();
            return Ok(books);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    //view book by id
    // GET: api/Library/{id}
    [HttpGet("{id}")]
    public ActionResult<int> GetBook(int id)
    {
        try
        {
            var result =_manageBook.ViewBook(id);

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
    public ActionResult<int> AddBook([FromBody] Book book)
    {
        try
        {
            if (book == null)
            {
                return BadRequest("Book data is required");
            }

            _manageBook.AddBook(book);


            //event status return

            var result = _leh.tempData["status"];

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
    public ActionResult<int> UpdateBook([FromBody] Book book)
    {
        try
        {
            if (book == null)
            {
                return BadRequest("Book data is required");
            }

           
            //trigger events 
            _manageBook.UpdateBook(book);


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
    public ActionResult<int> DeleteBook(int id)
    {
        try
        {
            _manageBook.DeleteBook(id);

            //event returning

            var result = _leh.tempData["Status"];
            return Ok(result);

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"internal server error: {ex.Message}");
        }
    }
}
