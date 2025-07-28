using BMS.BLL.Dto;
using BMS.DAL.Repository;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.Validation;

public class Validation : IValidation
{
    private IBookAccess<InsertBookDto> _iba;

    public Validation(IBookAccess<InsertBookDto> iba)
    {
        _iba = iba;
    }
    public int AuthorValidation(string author)
    {
        // Return codes: 0 = valid, 1 = null/empty, 2 = contains numbers, 3 = too short, 4 = too long
        if (string.IsNullOrWhiteSpace(author))
        {
            return 1; // Invalid: null, empty, or whitespace
        }

        if (author.Any(char.IsDigit))
        {
            return 2; // Invalid: contains numbers
        }

        if (author.Length < 2)
        {
            return 3; // Invalid: too short
        }

        if (author.Length > 100)
        {
            return 4; // Invalid: too long
        }

        return 0; // Valid
    }

 

    public int TitleValidation(string title)
    {
        // Return codes: 0 = valid, 1 = null/empty, 2 = too short, 3 = too long
        if (string.IsNullOrWhiteSpace(title))
        {
            return 1; // Invalid: null, empty, or whitespace
        }

        if (title.Length < 1)
        {
            return 2; // Invalid: too short
        }

        if (title.Length > 200)
        {
            return 3; // Invalid: too long
        }

        return 0; // Valid
    }

    public int YearValidation(int year)
    {
        // Return codes: 0 = valid, 1 = too old, 2 = future year
        int currentYear = DateTime.Now.Year;
        
        if (year < 1000)
        {
            return 1; // Invalid: too old (before year 1000)
        }

        if (year > currentYear + 1)
        {
            return 2; // Invalid: more than 1 year in the future
        }

        return 0; // Valid
    }



    public async Task<bool> DuplicationValidation(Book book)
    {
        //throw new NotImplementedException();

        var result = await _iba.BookExists(x=> 
                                                x.Title==book.Title &&
                                                x.PublishedYear==book.PublishedYear &&
                                                x.Author==book.Author);


        return result; //true; //Duplication found //false; //no Duplication
    }
}
