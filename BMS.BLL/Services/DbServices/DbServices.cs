using BMS.BLL.Services.EventHandlers;
using BMS.BLL.Services.Validation;
using BMS.DAL.Repository;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.DbServices;

public class DbServices : IDbServices<Book> 
{
    //event delegates

    public event Action<Book> BookoperationSucceeded;

    public event Action<int> BookDeletionSucceeded;

    public event Action<Book, int> ValidationFailed;

    private IValidation _ival;
    private IBookAccess<Book> _iba;
   
    public DbServices(IValidation ival, IBookAccess<Book> iba )
    {
        _ival = ival;
        _iba = iba;
      
    }

    public async Task AddBook(Book entity)
    {
        // Add validation before saving
        var authorValidation = _ival.AuthorValidation(entity.Author);
       

        var titleValidation = _ival.TitleValidation(entity.Title);


        var yearValidation = _ival.YearValidation(entity.PublishedYear);

        var ValidationErrors = authorValidation + titleValidation + yearValidation;

        if (yearValidation != 0 || titleValidation != 0 || authorValidation != 0)
        {
            ValidationFailed?.Invoke(entity, ValidationErrors);
            return;
        }

        await _iba.AddBook(entity);
        BookoperationSucceeded?.Invoke(entity);

    }

    public async Task DeleteBook(int id)
    {
        await _iba.DeleteBook(id);
        BookDeletionSucceeded?.Invoke(id);
        
    }

    public async Task UpdateBook(Book entity)
    {
       
        var authorValidation = _ival.AuthorValidation(entity.Author);

        var titleValidation = _ival.TitleValidation(entity.Title);

        var yearValidation = _ival.YearValidation(entity.PublishedYear);

        var ValidationErrors = authorValidation + titleValidation + yearValidation;

        if (yearValidation != 0 || titleValidation != 0 || authorValidation != 0)
        {
            ValidationFailed.Invoke(entity, ValidationErrors);
            return;
        }

        await _iba.UpdateBook(entity);
        BookoperationSucceeded?.Invoke(entity);
    }

    public async Task <IEnumerable<Book>> ViewAllBooks()
    {
        return await _iba.ViewAllBooks();
    }

    public async Task<Book> ViewBook(int id)
    {
        return await _iba.ViewBook(id);
        
    }
}
