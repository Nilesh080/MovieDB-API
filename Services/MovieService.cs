using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieValidation _movieValidation;
        private readonly IMovieRepository _movieRepository;
        private readonly IActorService _actorService;
        private readonly IProducerService _producerService;
        private readonly IGenreService _genreService;
        private readonly ISupabaseService _supabaseService;

        public MovieService(IActorService actorService, IProducerService producerService, IGenreService genreService, ISupabaseService supabaseService, IMovieValidation movieValidation, IMovieRepository movieRepository)
        {
            _movieValidation = movieValidation;
            _movieRepository = movieRepository;
            _actorService = actorService;
            _producerService = producerService;
            _genreService = genreService;
            _supabaseService = supabaseService;
        }

        public async Task<(string Message, int Id)> CreateAsync(MovieRequest request)
        {
            List<int> actorIds = request.ActorIds;
            List<int> genreIds = request.GenreIds;

            await _movieValidation.ValidateActorIdsAsync(actorIds);
            await _movieValidation.ValidateGenreIdsAsync(genreIds);
            await _movieValidation.ValidateProducerIdAsync(request.ProducerId);
            _movieValidation.ValidateRequest(request);

            Movie movieModel = new Movie
            {
                Name = request.Name,
                YearOfRelease = request.YearOfRelease,
                Plot = request.Plot,
                ProducerId = request.ProducerId,
                CoverImage = request.CoverImage
            };
            await _movieRepository.CreateAsync(movieModel, actorIds, genreIds);

            return ($"Movie '{movieModel.Name}' created successfully.", movieModel.Id);
        }

        public async Task<string> UpdateAsync(int id, MovieRequest request)
        {
            await ExistsAsync(id);
            await _movieValidation.ValidateActorIdsAsync(request.ActorIds);
            await _movieValidation.ValidateGenreIdsAsync(request.GenreIds);
            await _movieValidation.ValidateProducerIdAsync(request.ProducerId);
            _movieValidation.ValidateRequest(request);

            var movieModel = new Movie
            {
                Id = id,
                Name = request.Name,
                YearOfRelease = request.YearOfRelease,
                Plot = request.Plot,
                ProducerId = request.ProducerId,
                CoverImage = request.CoverImage
            };

            await _movieRepository.UpdateAsync(movieModel, request.ActorIds, request.GenreIds);

            return $"Movie id: '{id}' updated successfully.";
        }

        public async Task DeleteAsync(int id)
        {
            await ExistsAsync(id);
            await _movieRepository.DeleteAsync(id);
        }

        public async Task<List<MovieResponse>> GetAllAsync()
        {
            var moviesResponse = await _movieRepository.GetAllAsync();

            var movieTasks = moviesResponse
                .Select(response => MapToResponseAsync(response.Movie, response.Item2.ActorIds, response.Item2.GenreIds));

            return (await Task.WhenAll(movieTasks)).ToList();
        }

        public async Task<MovieResponse> GetByIdAsync(int id)
        {
            await ExistsAsync(id);
            var movie = await ExistsAsync(id);
            return await MapToResponseAsync(movie.Movie, movie.Item2.ActorIds, movie.Item2.GenreIds);
        }

        public async Task<List<MovieResponse>> GetAllAsync(int year)
        {
            var movies = await _movieRepository.GetAllAsync(year);
            var movieTasks = movies
                .Select(movie => MapToResponseAsync(movie.Movie, movie.Item2.ActorIds, movie.Item2.GenreIds));

            return (await Task.WhenAll(movieTasks)).ToList();
        }

        public async Task<string> UploadPosterAsync(int movieId, IFormFile posterImage)
        {
            var movie = await ExistsAsync(movieId);

            if (posterImage == null)
            {
                throw new ArgumentException("No file uploaded");
            }

            if (!string.IsNullOrEmpty(movie.Movie.CoverImage))
            {
                await _supabaseService.DeleteMoviePosterAsync(movie.Movie.CoverImage);
            }

            var posterUrl = await _supabaseService.UploadMoviePosterAsync(movieId, posterImage);

            await _movieRepository.UpdatePosterUrlAsync(movieId, posterUrl);

            return posterUrl;
        }

        public async Task<(Movie Movie, (List<int> ActorIds, List<int> GenreIds))> ExistsAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie.Movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {id} was not found.");
            }

            return movie;
        }

        private async Task<MovieResponse> MapToResponseAsync(Movie movie, List<int> actors, List<int> genres)
        {
            var actorTasks = actors
            .Select(async id =>
            {
                return await _actorService.GetByIdAsync(id);
            });

            var actorResponses = (await Task.WhenAll(actorTasks)).ToList();

            var producerResponse = await _producerService.GetByIdAsync(movie.ProducerId);

            var genreTasks = genres
            .Select(async id =>
            {
                return await _genreService.GetByIdAsync(id);
            });
            var genreResponses = (await Task.WhenAll(genreTasks)).ToList();

            return new MovieResponse
            {
                Id = movie.Id,
                Name = movie.Name,
                YearOfRelease = movie.YearOfRelease,
                Plot = movie.Plot,
                Producer = producerResponse,
                CoverImage = movie.CoverImage,
                Actors = actorResponses,
                Genres = genreResponses
            };
        }
    }
}