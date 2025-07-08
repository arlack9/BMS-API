
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

        // GET: api/Library/{id}
        [HttpGet("{id}")]
        public ActionResult<int> GetBook(int id)
        {
            try
            {
                var result = _manageBook.ViewBook(id);
                if (result <= 0)
                {
                    return NotFound($"Book with ID {id} not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _manageBook.AddBook(book);

                if (result <= 0)
                {
                    return BadRequest("Failed to add book");
                }

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Library/{id}
        [HttpPut("{id}")]
        public ActionResult<int> UpdateBook(int id, [FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("Book data is required");
                }

                if (id != book.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _manageBook.UpdateBook(book);

                if (result <= 0)
                {
                    return NotFound($"Book with ID {id} not found or update failed");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
