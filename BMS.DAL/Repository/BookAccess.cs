using BMS.DAL.DB;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Repository;

public class BookAccess : IBookAccess<Book>
{
    private ApplicationDbContext _context;
    public BookAccess(ApplicationDbContext context)
    {
        _context = context;   
    }
    public int AddBook(Book entity)
    {
        _context.Books.Add(entity);
        return _context.SaveChanges();//returns integer count of entries modified
    }

    public int DeleteBook(int id)
    {
        var book=_context.Books.Find(id);
        if(book==null)
        {
            return -1;
        }
        _context.Books.Remove(book);
        return _context.SaveChanges();

    }

    public int UpdateBook(Book entity)
    {
        _context.Books.Update(entity);
        return _context.SaveChanges();
    }

    public IEnumerable<Book> ViewAllBooks()
    {
        return _context.Books.ToList<Book>();
        
    }

    public Book ViewBook(int id) { 

        return _context.Books.Find(id);
    }

    // Book IBookAccess<Book>.ViewBook(int id) //no public keyword explicit interface implemtnation
    //{

    //    return _context.Books.Find(id);
    //}
}
