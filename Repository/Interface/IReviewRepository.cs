using IMDBApi_Assignment4.Models.DB;

namespace IMDBApi_Assignment4.Repository.Interface
{
    public interface IReviewRepository
    {
        Task<List<int>> GetByMovieIdAsync(int movieId);
        Task<Review> GetByIdAsync(int id);
        Task CreateAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);
        Task DeleteByMovieIdAsync(int movieId);
    }
}