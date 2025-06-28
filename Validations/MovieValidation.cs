using System.ComponentModel.DataAnnotations;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Validations
{
    public class MovieValidation : IMovieValidation
    {
        public readonly IActorService _actorService;
        public readonly IGenreService _genreService;
        public readonly IProducerService _producerService;

        public MovieValidation(IProducerService producerService, IGenreService genreService, IActorService actorService)
        {
            _actorService = actorService;
            _genreService = genreService;
            _producerService = producerService;
        }
        public async Task ValidateActorIdsAsync(List<int> actorIds)
        {
            await Task.WhenAll(actorIds.Select(id => _actorService.ExistsAsync(id)));
        }

        public async Task ValidateGenreIdsAsync(List<int> genreIds)
        {
            await Task.WhenAll(genreIds.Select(id => _genreService.ExistsAsync(id)));
        }

        public async Task ValidateProducerIdAsync(int producerId)
        {
            await _producerService.ExistsAsync(producerId);
        }

        public void ValidateRequest(MovieRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Movie request cannot be null");

            if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200 || request.Name.Length < 2)
                throw new ValidationException("Movie name must be between 2 and 200 characters.");

            if (request.YearOfRelease < 1900 || request.YearOfRelease > 2100)
                throw new ValidationException("Year of release must be between 1900 and 2100.");

            if (!string.IsNullOrWhiteSpace(request.Plot) && request.Plot.Length > 200)
                throw new ValidationException("Plot cannot exceed 200 characters.");

            if (request.ProducerId < 1)
                throw new ValidationException("ProducerId must be a positive number.");

            if (!string.IsNullOrWhiteSpace(request.CoverImage))
            {
                if (request.CoverImage.Length > 500)
                    throw new ValidationException("Cover image URL cannot exceed 500 characters.");

                if (!Uri.IsWellFormedUriString(request.CoverImage, UriKind.Absolute))
                    throw new ValidationException("Cover image must be a valid URL.");
            }

            if (request.ActorIds == null || request.ActorIds.Count < 1)
                throw new ValidationException("At least one actor must be selected.");

            if (request.GenreIds == null || request.GenreIds.Count < 1)
                throw new ValidationException("At least one genre must be selected.");
        }
    }
}
