namespace IMDBApi_Assignment4.Models.DTOs.Response
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public PersonResponse Producer { get; set; }
        public string CoverImage { get; set; }
        public List<PersonResponse> Actors { get; set; } = new List<PersonResponse>();
        public List<GenreResponse> Genres { get; set; } = new List<GenreResponse>();
    }
}
