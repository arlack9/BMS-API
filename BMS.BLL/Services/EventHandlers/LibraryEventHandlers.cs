using BMS.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.EventHandlers;

public class LibraryEventHandlers
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataFactory;

   
    public LibraryEventHandlers(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataFactory = tempDataFactory;
    }

    
    private ITempDataDictionary GetTempData()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            return _tempDataFactory.GetTempData(httpContext);
        }
        return null;
    }

    
    public void HandleBookOperationSuccess(Book book)
    {
        Console.WriteLine($"success: Book '{book.Title}' by {book.Author} operation completed!");
        
     
        var tempData = GetTempData();
        if (tempData != null)
        {
            tempData["SuccessMessage"] = $"Book '{book.Title}' operation completed successfully!";
        }
    }

    
    public void HandleBookDeletionSuccess(int bookId)
    {
        Console.WriteLine($"Success: Book ID {bookId} deleted successfully!");
        
       
        var tempData = GetTempData();
        if (tempData != null)
        {
            tempData["SuccessMessage"] = $"Book with ID {bookId} deleted successfully!";
        }
    }

  
    public void HandleValidationFailure(Book book, int no)
    {
        Console.WriteLine($"Validation failed for '{book.Title}' ");
        
       
        var tempData = GetTempData();
        if (tempData != null)
        {
        
            tempData["ValidationErrors"] = $"Validation failed for '{book.Title}'";
        }
    }



}
