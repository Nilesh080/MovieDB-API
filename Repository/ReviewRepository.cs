using Dapper;
using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Repository.Interface;

namespace IMDBApi_Assignment4.Repository
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString("IMDBDB"), "Reviews")
        {
        }

        public async Task CreateAsync(Review review)
        {
            const string sql = @"
                INSERT INTO Reviews (
                    Message,
                    MovieId
                ) VALUES (
                    @Message,
                    @MovieId
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            review.Id = await ExecuteScalarAsync<int>(sql, review);
        }

        public async Task DeleteByMovieIdAsync(int movieId)
        {
            const string sql = @"
                DELETE FROM Reviews 
                WHERE MovieId = @MovieId";

            await ExecuteAsync(sql, new { MovieId = movieId });
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM Reviews 
                WHERE Id = @Id";

            await ExecuteAsync(sql, new { Id = id });
        }
        public async Task<List<int>> GetByMovieIdAsync(int movieId)
        {
            const string sql = @"
                SELECT Id 
                FROM Reviews 
                WHERE MovieId = @MovieId";

            var reviewIds = await QueryAsync<int>(sql, new { MovieId = movieId });
            return reviewIds.AsList();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT * 
                FROM Reviews 
                WHERE Id = @Id";

            return await QuerySingleOrDefaultAsync(sql, new { Id = id });
        }
        public async Task UpdateAsync(Review review)
        {
            const string sql = @"
                UPDATE Reviews
                SET Message = @Message
                WHERE Id = @Id";

            await ExecuteAsync(sql, review);
        }
    }
}