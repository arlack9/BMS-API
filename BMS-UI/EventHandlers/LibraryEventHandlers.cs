using BMS.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS_UI.EventHandlers;

public class LibraryEventHandlers
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataFactory;


    public LibraryEventHandlers(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataFactory = tempDataFactory;
    }


    public ITempDataDictionary GetTempData()
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
        //Console.WriteLine($"success: Book '{book.Title}' by {book.Author} operation completed!");
        
     
        var tempData = GetTempData();
        if (tempData != null)
        {
            tempData["Status"] = $"Book '{book.Title}' operation completed successfully!";
            
        }
    }

    
    public void HandleBookDeletionSuccess(Book book)
    {
        //Console.WriteLine($"Success: Book {book.Id} deleted successfully!");
        
       
        var tempData = GetTempData();
        if (tempData != null)
        {
            tempData["Status"] = $"Book with Title {book.Title} deleted successfully!";
        }
    }


    public void HandleValidationFailure(Book book, int result)
    {
        //Console.WriteLine($"Validation failed for '{book.Title}' , errorcode{result} ");
        
       
        var tempData = GetTempData();
        if (tempData != null)
        {
        
            tempData["Status"] = $"Validation failed for '{book.Title}',errorcode{result}";
        }
    }


    public void HandleBookUpdationSuccess(Book book)
    {
        var tempData = GetTempData();
        if (tempData != null)
        {
            tempData["Status"] = $"Book '{book.Title}' Updation Success";
        }

    }

    public void HandleBookAdditionSuccess(Book book)
    {
        var tempData=GetTempData();
        if(tempData != null)
        {
            tempData["Status"] = $"Book '{book.Title}' Updation Success";
        }
    }
    
}
