using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;

namespace IMDBApi_Assignment4.Services.Interface
{
    public interface IReviewService
    {
        Task<List<ReviewResponse>> GetByMovieIdAsync(int movieId);
        Task<ReviewResponse> GetByIdAsync(int id, int movieId);
        Task<(string Message, int Id)> CreateAsync(int movieId, ReviewRequest reviewRequest);
        Task<string> UpdateAsync(int movieId, int id, ReviewRequest reviewRequest);
        Task DeleteAsync(int movieId, int id);
        Task DeleteAllByMovieIdAsync(int movieId);
        Task<Review> ExistsAsync(int id);
        Task ValidateReviewBelongsToMovieAsync(int id, int movieId);
    }
}
