using BMS.BLL.Services.Validation;
using BMS.DAL.Repository;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services;

public class DbServices : IDbServices<Book> 
{
    private IValidation _ival;
    private IBookAccess<Book> _iba;
    public DbServices(IValidation ival, IBookAccess<Book> iba)
    {
        _ival = ival;
        _iba = iba;
    }

    public int AddBook(Book entity)
    {
        // Add validation before saving
        var authorValidation = _ival.AuthorValidation(entity.Author);
        if (authorValidation != 0)
        {
            return -1; // Invalid author
        }

        var titleValidation = _ival.TitleValidation(entity.Title);
        if (titleValidation != 0)
        {
            return -2; // Invalid title
        }

        var yearValidation = _ival.YearValidation(entity.PublishedYear);
        if (yearValidation != 0)
        {
            return -3; // Invalid year
        }

        return _iba.AddBook(entity);
    }

    public int DeleteBook(int id)
    {
        return _iba.DeleteBook(id);
    }

    public int UpdateBook(Book entity)
    {
        // Add validation before updating
        var authorValidation = _ival.AuthorValidation(entity.Author);
        if (authorValidation != 0)
        {
            return -1; // Invalid author
        }

        var titleValidation = _ival.TitleValidation(entity.Title);
        if (titleValidation != 0)
        {
            return -2; // Invalid title
        }

        var yearValidation = _ival.YearValidation(entity.PublishedYear);
        if (yearValidation != 0)
        {
            return -3; // Invalid year
        }

        return _iba.UpdateBook(entity);
    }

    public IEnumerable<Book> ViewAllBooks()
    {
        return _iba.ViewAllBooks();
    }

    public int ViewBook(int id)
    {
        var book = _iba.ViewBook(id);
        return book != null ? 1 : 0; // Return 1 if found, 0 if not found
    }
}
