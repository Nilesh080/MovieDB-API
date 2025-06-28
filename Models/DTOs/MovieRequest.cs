using System.ComponentModel.DataAnnotations;

namespace IMDBApi_Assignment4.Models.DTOs.Request
{
    public class MovieRequest
    {
        [Required(ErrorMessage = "Movie name is required.")]
        public string Name { get; set; }

        public int YearOfRelease { get; set; }

        public string Plot { get; set; }

        [Required(ErrorMessage = "ProducerId is required.")]
        public int ProducerId { get; set; }

        public string CoverImage { get; set; }

        [Required(ErrorMessage = "At least one actor is required.")]
        public List<int> ActorIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "At least one genre is required.")]
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}