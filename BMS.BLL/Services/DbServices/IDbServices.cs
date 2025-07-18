using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.DbServices;

public interface IDbServices<T> where T : class
{

    public Task AddBook(T entity);
    public Task DeleteBook(int id);
    public Task UpdateBook(T entity);
    public Task<IEnumerable<T>> ViewAllBooks();
    public Task<Book> ViewBook(int id);

    public event Action<Book> BookoperationSucceeded;

    public event Action<int> BookDeletionSucceeded;

    public event Action<Book, int> ValidationFailed;
}
