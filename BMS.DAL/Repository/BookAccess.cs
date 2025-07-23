using BMS.DAL.DB;
using BMS.Models.Models;
using Microsoft.EntityFrameworkCore;
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
    public async Task AddBook(Book entity)
    {
        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();//returns integer count of entries modified
    }

    public async Task DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        var res =   _context.Books.Remove(book);
        
      await _context.SaveChangesAsync();
       

    }
     
    public async Task UpdateBook(Book entity)
    {
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();
    }
     
    public async Task <IEnumerable<Book>> ViewAllBooks()
    {
        return await _context.Books.ToListAsync();
        
    }
     
    public async Task <Book>ViewBook(int id) { 

        return await _context.Books.FindAsync(id);
    }

    //added book search
    //added search using LINQ
    public async Task<IEnumerable<Book>> BookSearch(string keywords)
    {
        if (string.IsNullOrWhiteSpace(keywords))
        {
            return await ViewAllBooks();
        }

        return await _context.Books
            .Where(b => b.Title.Contains(keywords) ||
                       b.Author.Contains(keywords) ||
                       b.PublishedYear.ToString().Contains(keywords))
            .ToListAsync();
    }



    // Book IBookAccess<Book>.ViewBook(int id) //no public keyword explicit interface implemtnation
    //{public   
    //    return _context.Books.Find(id);
    //}
}
