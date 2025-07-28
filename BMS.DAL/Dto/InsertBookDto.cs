using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.DAL.Dto;

public class InsertBookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int PublishedYear { get; set; }
}
