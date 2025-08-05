//using BMS.BLL.Dto;
using BMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services.Validation
{
    public interface IValidation
    {
        public int TitleValidation(string title);
        public int AuthorValidation(string author);
        public int YearValidation(int year);

        public Task<bool> DuplicationValidation(Book book);
        
    }
}
