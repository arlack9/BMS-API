using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.Events;

public class Events : IEvents
{
    public event Action<Book> BookoperationSucceeded;
    public event Action<Book> BookAddSucceeded;
    public event Action<Book> BookupdationSucceeded;
    public event Action<Book> BookDeletionSucceeded;
    public event Action<Book, int> ValidationFailed;

    public void OnBookoperationSucceeded(Book book)
    => BookoperationSucceeded?.Invoke(book);

    public void OnBookAddSucceeded(Book book)
        => BookAddSucceeded?.Invoke(book);

    public void OnBookupdationSucceeded(Book book)
        => BookupdationSucceeded?.Invoke(book);

    public void OnBookDeletionSucceeded(Book book)
        => BookDeletionSucceeded?.Invoke(book);

    public void OnValidationFailed(Book book, int errorCode)
        => ValidationFailed?.Invoke(book, errorCode);

 

    //protected access modifier -> only child class access it.
    
}
