using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Repository.Interface;

namespace IMDBApi_Assignment4.Repository
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString("IMDBDB"), "Genre")
        {
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Name 
                FROM Genres";

            var result = await QueryAsync(sql);
            return result.ToList();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Name 
                FROM Genres 
                WHERE Id = @Id";

            return await QuerySingleOrDefaultAsync(sql, new { Id = id });
        }

        public async Task CreateAsync(Genre genre)
        {
            const string sql = @"
                INSERT INTO Genres (Name)
                VALUES (@Name);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await ExecuteScalarAsync<int>(sql, new { genre.Name });
            genre.Id = id;
        }

        public async Task<bool> UpdateAsync(Genre genre)
        {
            const string sql = @"
                UPDATE Genres 
                SET Name = @Name 
                WHERE Id = @Id";

            var result = await ExecuteAsync(sql, new { genre.Id, genre.Name });
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM Genres 
                WHERE Id = @Id";

            var result = await ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
}