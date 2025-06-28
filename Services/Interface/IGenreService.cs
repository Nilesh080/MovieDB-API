using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;

namespace IMDBApi_Assignment4.Services.Interface
{
    public interface IGenreService
    {
        Task<List<GenreResponse>> GetAllAsync();
        Task<GenreResponse> GetByIdAsync(int id);
        Task<(string Message, int Id)> CreateAsync(GenreRequest genreRequest);
        Task<string> UpdateAsync(int id, GenreRequest genreRequest);
        Task DeleteAsync(int id);
        Task<Genre> ExistsAsync(int id);
    }
}