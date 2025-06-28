using System.Data;
using Dapper;
using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Repository.Interface;

namespace IMDBApi_Assignment4.Repository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString("IMDBDB"), "Movies")
        {
        }
        public async Task CreateAsync(Movie movie, List<int> actorIds, List<int> genreIds)
        {
            var actorIdsString = string.Join(",", actorIds);
            var genreIdsString = string.Join(",", genreIds);

            var parameters = new DynamicParameters();
            parameters.Add("@Name", movie.Name);
            parameters.Add("@YearOfRelease", movie.YearOfRelease);
            parameters.Add("@Plot", movie.Plot);
            parameters.Add("@ProducerId", movie.ProducerId);
            parameters.Add("@CoverImage", movie.CoverImage);
            parameters.Add("@ActorIds", actorIdsString);
            parameters.Add("@GenreIds", genreIdsString);
            parameters.Add("@MovieId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await ExecuteStoredProcedureAsync("usp_AddMovie", parameters);
            movie.Id = parameters.Get<int>("@MovieId");
        }

        public async Task DeleteAsync(int movieId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MovieId", movieId);

            await ExecuteStoredProcedureAsync("usp_DeleteMovie", parameters);
        }

        public async Task<List<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))>> GetAllAsync()
        {
            var movies = (await QueryAsync("SELECT * FROM Movies")).ToList();

            if (!movies.Any())
                return new List<(Movie, (List<int>, List<int>))>();

            var movieIds = movies.Select(m => m.Id).ToList();

            var actorMappings = (await QueryAsync<(int MovieId, int ActorId)>(
                @"SELECT MovieId, ActorId 
                FROM MoviesActors 
                WHERE MovieId IN @MovieIds",
                new { MovieIds = movieIds }
            )).ToList();

            var genreMappings = (await QueryAsync<(int MovieId, int GenreId)>(
                @"SELECT MovieId, GenreId 
                FROM MoviesGenres 
                WHERE MovieId IN @MovieIds",
                new { MovieIds = movieIds }
            )).ToList();

            var results = movies.Select(movie => (
                movie,
                (
                    actorMappings
                        .Where(a => a.MovieId == movie.Id)
                        .Select(a => a.ActorId)
                        .ToList(),

                    genreMappings
                        .Where(g => g.MovieId == movie.Id)
                        .Select(g => g.GenreId)
                        .ToList()
                )
            )).ToList();

            return results;
        }

        public async Task<List<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))>> GetAllAsync(int year)
        {
            var movies = (await QueryAsync<Movie>(
                @"SELECT * 
                FROM Movies 
                WHERE YearOfRelease = @Year",
                new { Year = year }
            )).ToList();

            if (!movies.Any())
                return new List<(Movie, (List<int>, List<int>))>();

            var movieIds = movies.Select(m => m.Id).ToList();

            var actorMappings = (await QueryAsync<(int MovieId, int ActorId)>(
                @"SELECT MovieId, ActorId 
                FROM MoviesActors 
                WHERE MovieId IN @MovieIds",
                new { MovieIds = movieIds }
            )).ToList();

            var genreMappings = (await QueryAsync<(int MovieId, int GenreId)>(
                @"SELECT MovieId, GenreId 
                FROM MoviesGenres 
                WHERE MovieId IN @MovieIds",
                new { MovieIds = movieIds }
            )).ToList();

            var results = movies.Select(movie => (
                movie,
                (
                    actorMappings
                        .Where(a => a.MovieId == movie.Id)
                        .Select(a => a.ActorId)
                        .ToList(),

                    genreMappings
                        .Where(g => g.MovieId == movie.Id)
                        .Select(g => g.GenreId)
                        .ToList()
                )
            )).ToList();

            return results;
        }

        public async Task<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))> GetByIdAsync(int id)
        {
            var movie = await QuerySingleOrDefaultAsync(
                @"SELECT * 
                FROM Movies 
                WHERE Id = @Id",
                new { Id = id }
            );

            var actorIds = (await QueryAsync<int>(
                @"SELECT ActorId 
                FROM MoviesActors 
                WHERE MovieId = @MovieId",
                new { MovieId = id }
            )).ToList();

            var genreIds = (await QueryAsync<int>(
                @"SELECT GenreId 
                FROM MoviesGenres 
                WHERE MovieId = @MovieId",
                new { MovieId = id }
            )).ToList();

            return (movie, (actorIds, genreIds));
        }

        public async Task UpdateAsync(Movie movie, List<int> actorIds, List<int> genreIds)
        {
            var actorIdsString = string.Join(",", actorIds);
            var genreIdsString = string.Join(",", genreIds);

            var parameters = new DynamicParameters();
            parameters.Add("@MovieId", movie.Id);
            parameters.Add("@Name", movie.Name);
            parameters.Add("@YearOfRelease", movie.YearOfRelease);
            parameters.Add("@Plot", movie.Plot);
            parameters.Add("@ProducerId", movie.ProducerId);
            parameters.Add("@CoverImage", movie.CoverImage);
            parameters.Add("@ActorIds", actorIdsString);
            parameters.Add("@GenreIds", genreIdsString);

            await ExecuteStoredProcedureAsync("usp_UpdateMovie", parameters);
        }

        public async Task<bool> UpdatePosterUrlAsync(int movieId, string posterUrl)
        {
            const string sql = @"
                UPDATE Movies
                SET CoverImage = @CoverImage
                WHERE Id = @Id";

            var rowsAffected = await ExecuteAsync(sql, new { Id = movieId, CoverImage = posterUrl });
            return rowsAffected > 0;
        }
    }
}