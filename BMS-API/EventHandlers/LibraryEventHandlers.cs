using BMS.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS_API.EventHandlers;

public class LibraryEventHandlers
{
  public Dictionary<string,string> tempData =new();
    
    public void HandleBookOperationSuccess(Book book)
    {
        //Console.WriteLine($"success: Book '{book.Title}' by {book.Author} operation completed!");

        var tempData = new Dictionary<string, string>();
        //var tempData = GetTempData();
        if (tempData != null)
        {
            tempData["Status"] = $"Book '{book.Title}' operation completed successfully!";
            
        }
    }

    
    public void HandleBookDeletionSuccess(Book book)
    {
        //Console.WriteLine($"Success: Book {book.Id} deleted successfully!");

        if (tempData != null)
        {
            tempData["Status"] = $"Book with Title {book.Title} deleted successfully!";
        }
    }


    public void HandleValidationFailure(Book book, int result)
    {
        //Console.WriteLine($"Validation failed for '{book.Title}' , errorcode{result} ");
        
        if (tempData != null)
        {
        
            tempData["Status"] = $"Validation failed for '{book.Title}',errorcode{result}";
        }
    }


    public void HandleBookUpdationSuccess(Book book)
    {
        if (tempData != null)
        {
            tempData["Status"] = $"Book '{book.Title}' Updation Success";
        }

    }

    public void HandleBookAdditionSuccess(Book book)
    {
        if(tempData != null)
        {
            tempData["Status"] = $"Book '{book.Title}' Updation Success";
        }
    }
    
}
