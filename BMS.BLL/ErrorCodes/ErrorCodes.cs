using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace BMS.BLL.ErrorCodes;

public static class ErrorCodes
{
    public const string AuthorNullOrEmpty = "A100";
    public const string AuthorContainsNumbers = "A200";
    public const string TitleTooShort = "T20";
    public const string TitleTooLong = "T30";
    public const string AuthorTooShort = "A40";
    public const string AuthorTooLong = "A50";
    public const string TitleNullOrEmpty = "T70";
    public const string YearTooOld = "Y90";
    public const string YearInFuture = "Y100";
    public const string BookDuplicationFound = "D100";


    public static readonly Dictionary<string, string> ErrorsDictionary = new()
    {

        //validation errors
        { AuthorNullOrEmpty, "Author name cannot be empty." },
        { AuthorContainsNumbers, "Author name cannot contain numbers." },
        { AuthorTooShort, "Author name is too short." },
        { AuthorTooLong, "Author name is too long." },

        { TitleTooShort, "Title is too short." },
        {TitleNullOrEmpty, "Title cannot be empty." },
        { TitleTooShort, "Title is too short." },
        {TitleTooLong, "Title is too long." },

        { YearTooOld, "Year is too far in the past." },
        { YearInFuture, "Year cannot be in the future." }
    };
}



