using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BMS.BLL.Services.Events;

public interface IEvents
{
    event Action<Book> BookoperationSucceeded;
    event Action<Book> BookAddSucceeded;
    event Action<Book> BookupdationSucceeded;
    event Action<Book> BookDeletionSucceeded;
    event Action<Book, int> ValidationFailed;

    void OnBookoperationSucceeded(Book book);
    void OnBookAddSucceeded(Book book);
    void OnBookupdationSucceeded(Book book);
    void OnBookDeletionSucceeded(Book book);
    void OnValidationFailed(Book book, int errorCode);
   

}