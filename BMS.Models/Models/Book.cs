using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Models.Models;

public class Book
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 20 characters.")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Author is required.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Author name must be between 3 and 20 characters.")]
    public string Author { get; set; } = "";

    [Required(ErrorMessage = "Published Year is required.")]
    public int PublishedYear { get; set; }
}
