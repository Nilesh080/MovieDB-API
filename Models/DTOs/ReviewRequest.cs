using System.ComponentModel.DataAnnotations;

namespace IMDBApi_Assignment4.Models.DTOs.Request
{
    public class ReviewRequest
    {
        [Required(ErrorMessage = "Review message is required")]
        public string Message { get; set; }
    }
}
