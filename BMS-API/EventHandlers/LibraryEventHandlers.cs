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
  public Dictionary<string,string> tempData =new() {  };
    
    public void HandleBookOperationSuccess(Book book)
    {
        //Console.WriteLine($"success: Book '{book.Title}' by {book.Author} operation completed!");


        //var tempData = GetTempData();

            tempData["status"] = $"Book '{book.Title}' operation completed successfully!";
            

    }

    
    public void HandleBookDeletionSuccess(Book book)
    {
        //Console.WriteLine($"Success: Book {book.Id} deleted successfully!");

            tempData["status"] = $"Book with Title {book.Title} deleted successfully!";
    
    }


    public void HandleValidationFailure(Book book, int result)
    {
        //Console.WriteLine($"Validation failed for '{book.Title}' , errorcode{result} ");
        
        
            tempData["status"] = $"Validation failed for '{book.Title}',errorcode{result}";
   
    }


    public void HandleBookUpdationSuccess(Book book)
    {
 
            tempData["status"] = $"Book '{book.Title}' Updation Success";


    }

    public void HandleBookAdditionSuccess(Book book)
    {

            tempData["status"] = $"Book '{book.Title}' Added Successfully";
        
    }
    
}
