
//using BMS.BLL.Dto;
using BMS.BLL.Services.Validation;
using BMS.DAL.Repository;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.DbServices;

public class DbServices : IDbServices<Book> 
{
    //event delegates
    //public event Action<Book> Angel;
    public event Action<Book> BookoperationSucceeded;

    public event Action<Book> BookAddSucceeded;

    public event Action<Book> BookupdationSucceeded;

    public event Action<Book> BookDeletionSucceeded;

    public event Action<Book, int> ValidationFailed;

    private IValidation _ival;
    private IBookAccess<Book> _iba;
   
    public DbServices(IValidation ival, IBookAccess<Book> iba )
    {
        _ival = ival;
        _iba = iba;
    }

    //Event operations
    public async Task AddBook(Book entity)
    {
        // Add validation before saving

        //pass this model to the class completely - objective
        var authorValidation = _ival.AuthorValidation(entity.Author);
        var titleValidation = _ival.TitleValidation(entity.Title);
        var yearValidation = _ival.YearValidation(entity.PublishedYear);
        var ValidationErrors = authorValidation + titleValidation + yearValidation;

        //check duplication
        var DuplicationCheck = await _ival.DuplicationValidation(entity);


        if (yearValidation != 0 || titleValidation != 0 || authorValidation != 0 || DuplicationCheck is true)
        {
            ValidationFailed?.Invoke(entity, ValidationErrors);
            return;
        }

        //db apply
        await _iba.AddBook(entity);
        BookAddSucceeded?.Invoke(entity);

    }

    public async Task DeleteBook(int id)
    {
        var DeletionBook = await _iba.ViewBook(id);

        await _iba.DeleteBook(id);
        
        BookDeletionSucceeded?.Invoke(DeletionBook);
        
    }

    public async Task UpdateBook(Book entity)
    {
       
        var authorValidation = _ival.AuthorValidation(entity.Author);

        var titleValidation = _ival.TitleValidation(entity.Title);

        var yearValidation = _ival.YearValidation(entity.PublishedYear);

        var ValidationErrors = authorValidation + titleValidation + yearValidation;

        var DuplicationCheck = await _ival.DuplicationValidation(entity);

        if (yearValidation != 0 || titleValidation != 0 || authorValidation != 0 || DuplicationCheck is true)
        {
            ValidationFailed.Invoke(entity, ValidationErrors);
            return;
        }

        await _iba.UpdateBook(entity);
        //BookoperationSucceeded?.Invoke(entity);
        BookupdationSucceeded?.Invoke(entity);
    }



    //Eventless operations

    public async Task <IEnumerable<Book>> ViewAllBooks()
    {
        return await _iba.ViewAllBooks();
    }

    public async Task<Book> ViewBook(int id)
    {
        return await _iba.ViewBook(id);
        
    }


    public async Task<IEnumerable<Book>> SearchBooks(string keywords)
    {
        //throw new NotImplementedException();
        return await _iba.SearchBooks(x=> 
                                            x.Title.Contains(keywords) ||
                                            x.Author.Contains(keywords) ||
                                            x.PublishedYear.ToString().Contains(keywords));


        //predicate passed like x=> x.Title.Contains(keywords) ||x.Author.Contains(keywords) ||x.PublishedYear.ToString().Contains(keywords)
    }


    

}
