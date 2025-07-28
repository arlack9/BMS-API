using BMS.DAL.DB;
using BMS.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    //Addbook
    public async Task AddBook(Book entity)
    {
        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();//returns integer count of entries modified
    }


    //Delete book by id
    public async Task DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        var res =   _context.Books.Remove(book);
        
      await _context.SaveChangesAsync();
       

    }
     

    //Update Book 
    public async Task UpdateBook(Book entity)
    {
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();
    }
     
    //view all books
    public async Task <IEnumerable<Book>> ViewAllBooks()
    {
        return await _context.Books.ToListAsync();
        
    }
     
    //get book by id
    public async Task <Book>ViewBook(int id) { 

        return await _context.Books.FindAsync(id);
    }

 



    //Check whether book exist return boolean , takes linq predicate
    public async Task <bool> BookExists(Expression<Func<Book,bool>> predicate)
    {
        return await _context.Books.AnyAsync(predicate);
    }

    //check using LINQ from BLL and return Books List
    public async Task<List<Book>> SearchBooks(Expression<Func<Book, bool>> predicate)
    {
        return await _context.Books.Where(predicate).ToListAsync();
    }


    //deprecated moved to BLL
    //added book search using LINQ
    //public async Task<IEnumerable<Book>> BookSearch(string keywords)
    //{
    //    if (string.IsNullOrWhiteSpace(keywords))
    //    {
    //        return await ViewAllBooks();
    //    }

    //    return await _context.Books
    //        .Where(b => b.Title.Contains(keywords) ||
    //                   b.Author.Contains(keywords) ||
    //                   b.PublishedYear.ToString().Contains(keywords))
    //        .ToListAsync();
    //}
}
