using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IGenreValidation _genreValidation;

        public GenreService(IGenreRepository genreRepository, IGenreValidation genreValidation)
        {
            _genreRepository = genreRepository;
            _genreValidation = genreValidation;
        }

        public async Task<List<GenreResponse>> GetAllAsync()
        {
            var genres = await _genreRepository.GetAllAsync();
            return genres.Select(MapToResponse).ToList();
        }

        public async Task<GenreResponse> GetByIdAsync(int id)
        {
            _genreValidation.ValidateIdAsync(id);

            var genre = await ExistsAsync(id);

            return MapToResponse(genre);
        }

        public async Task<(string Message, int Id)> CreateAsync(GenreRequest request)
        {
            _genreValidation.ValidateRequestAsync(request);

            var existingGenre = (await _genreRepository.GetAllAsync())
                                .FirstOrDefault(g => g.Name.Equals(request.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingGenre != null)
            {
                throw new ArgumentException($"Genre '{request.Name}' already exists");
            }

            var genre = new Genre
            {
                Name = request.Name
            };

            await _genreRepository.CreateAsync(genre);

            return ($"Genre '{genre.Name}' created successfully.", genre.Id);
        }

        public async Task<string> UpdateAsync(int id, GenreRequest request)
        {
            _genreValidation.ValidateIdAsync(id);

            var existingGenre = await ExistsAsync(id);

            _genreValidation.ValidateRequestAsync(request);

            var genre = (await _genreRepository.GetAllAsync())
                                .FirstOrDefault(g => g.Name.Equals(request.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (genre != null)
            {
                throw new ArgumentException($"Genre '{request.Name}' already exists");
            }

            existingGenre.Name = request.Name;

            await _genreRepository.UpdateAsync(existingGenre);

            return $"Genre id: '{id}' updated successfully.";
        }

        public async Task DeleteAsync(int id)
        {
            _genreValidation.ValidateIdAsync(id);

            var genre = await ExistsAsync(id);

            await _genreRepository.DeleteAsync(id);
        }

        public async Task<Genre> ExistsAsync(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);

            if (genre == null)
            {
                throw new KeyNotFoundException($"Genre with ID {id} not found");
            }

            return genre;
        }

        private GenreResponse MapToResponse(Genre genre)
        {
            return new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }
    }
}