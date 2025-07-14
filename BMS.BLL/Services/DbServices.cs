using BMS.BLL.Services.Validation;
using BMS.DAL.Repository;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public async Task AddBook(Book entity)
    {
        // Add validation before saving
        var authorValidation = _ival.AuthorValidation(entity.Author);
       

        var titleValidation = _ival.TitleValidation(entity.Title);
       

        var yearValidation = _ival.YearValidation(entity.PublishedYear);
   

         await _iba.AddBook(entity);
    }

    public async Task DeleteBook(int id)
    {
        await _iba.DeleteBook(id);
    }

    public async Task UpdateBook(Book entity)
    {
       
        var authorValidation = _ival.AuthorValidation(entity.Author);
       

        var titleValidation = _ival.TitleValidation(entity.Title);
       

        var yearValidation = _ival.YearValidation(entity.PublishedYear);
       

        await _iba.UpdateBook(entity);
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
