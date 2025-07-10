
using BMS.BLL.Services;
using BMS.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly IDbServices<Book> _manageBook;



        public LibraryController(IDbServices<Book> idb)
        {
            _manageBook = idb;
        }


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
                var result = _manageBook.ViewBook(id);
                if (result ==null)
                {
                    return NotFound($"Book with ID {id} not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error: {ex.Message}");
            }
        }

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

                

                var result = _manageBook.AddBook(book);

                if (result <= 0)
                {
                    return BadRequest("Book failed to add!");
                }

                return Ok("Book Added successfully");
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



                var result = _manageBook.UpdateBook(book);

                if (result <= 0)
                {
                    return NotFound($"Book not updated");
                }

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
                var result = _manageBook.DeleteBook(id);

                if (result <= 0)
                {
                    return NotFound($"Book with ID {id} not found");
                }

                return Ok("Book Successfully Deleted!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error: {ex.Message}");
            }
        }
    }
}
