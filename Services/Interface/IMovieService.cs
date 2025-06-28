using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;

namespace IMDBApi_Assignment4.Services.Interface
{
    public interface IMovieService
    {
        Task<List<MovieResponse>> GetAllAsync();
        Task<List<MovieResponse>> GetAllAsync(int year);
        Task<MovieResponse> GetByIdAsync(int id);
        Task<(string Message, int Id)> CreateAsync(MovieRequest movie);
        Task<string> UpdateAsync(int id, MovieRequest movieRequest);
        Task DeleteAsync(int id);
        Task<string> UploadPosterAsync(int movieId, IFormFile posterImage);
        Task<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))> ExistsAsync(int id);
    }
}
