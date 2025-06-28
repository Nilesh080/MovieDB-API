using System.ComponentModel.DataAnnotations;

namespace IMDBApi_Assignment4.Models.DTOs.Request
{
    public class GenreRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
