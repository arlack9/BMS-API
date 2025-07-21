using System.ComponentModel.DataAnnotations;

namespace BMS_API.Dto
{
    public class BookDto
    {
        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        public int PublishedYear { get; set; }

    }
}
