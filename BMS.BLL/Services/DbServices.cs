using BMS.BLL.Services.Validation;
using BMS.DAL.Repository;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services;

internal class DbServices : IDbServices<Book>
{
    private IValidation _ival;
    private IBookAccess<Book> _iba;
    public DbServices(IValidation ival, IBookAccess<Book> iba)
    {
        _ival = ival;
        _iba = iba;
    }

    //public Add Book

    public int AddBook(Book entity)
    {
        return _iba.AddBook(entity);

    }

    public int DeleteBook(int id)
    {
        throw new NotImplementedException();
    }

    public int UpdateBook(Book entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Book> ViewAllBooks()
    {
        throw new NotImplementedException();
    }

    public int ViewBook(int id)
    {
        throw new NotImplementedException();
    }
}
