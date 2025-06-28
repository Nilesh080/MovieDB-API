using System.ComponentModel.DataAnnotations;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Validations
{
    public class GenreValidation : IGenreValidation
    {
        public void ValidateIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Genre ID must be greater than zero");
        }

        public void ValidateRequestAsync(GenreRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Genre request cannot be null");

            if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 2 || request.Name.Length > 50)
                throw new ValidationException("Genre name must be between 2 and 50 characters");
        }
    }
}
