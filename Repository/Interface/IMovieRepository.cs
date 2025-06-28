using IMDBApi_Assignment4.Models.DB;

namespace IMDBApi_Assignment4.Repository.Interface
{
    public interface IMovieRepository
    {
        Task<List<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))>> GetAllAsync();
        Task<List<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))>> GetAllAsync(int year);
        Task<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))> GetByIdAsync(int id);
        Task CreateAsync(Movie movie, List<int> actorIds, List<int> genreIds);
        Task UpdateAsync(Movie movie, List<int> actorIds, List<int> genreIds);
        Task DeleteAsync(int id);
        Task<bool> UpdatePosterUrlAsync(int movieId, string posterUrl);
    }
}
