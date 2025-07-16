using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.EventHandlers;

public class LibraryEventHandlers
{

    public void HandleSuccess(Book book)
    {
        Console.WriteLine($"Book '{book.Title}' operation succeeded!");
    }

    public void HandleFailure(int no)
    {
        Console.WriteLine($"Book failed operation ");
    }

    public void HandleValidationFailure(Book book,int no)
    {
        Console.WriteLine($"Book Validation failed");
       
    }
}
