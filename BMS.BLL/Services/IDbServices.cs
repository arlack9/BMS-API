using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services;

public interface IDbServices<T> where T : class
{

    public int AddBook(T entity);
    public int DeleteBook(int id);
    public int UpdateBook(T entity);
    public IEnumerable<T> ViewAllBooks();
    public Book ViewBook(int id);

}
