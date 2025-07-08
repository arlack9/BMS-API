using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Repository;

public interface IBookAccess<T> where T : class
{

    public int AddBook(T entity);
    public int DeleteBook(int id);
    public int UpdateBook(T entity);
    public IEnumerable<T> ViewAllBooks();
    public T ViewBook(int id);

}
