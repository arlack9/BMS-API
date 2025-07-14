using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Repository;

public interface IBookAccess<T> where T : class
{

    public Task AddBook(T entity);
    public Task DeleteBook(int id);
    public Task UpdateBook(T entity);
    public Task<IEnumerable<T>> ViewAllBooks();
    public Task<T> ViewBook(int id);

}
