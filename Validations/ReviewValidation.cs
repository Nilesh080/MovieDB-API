using System.ComponentModel.DataAnnotations;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Validations
{
    public class ReviewValidation : IReviewValidation
    {
        public void ValidateMovieIdAsync(int movieId)
        {
            if (movieId <= 0)
                throw new ArgumentException("Movie ID must be greater than zero");
        }

        public void ValidateIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Review ID must be greater than zero");
        }

        public void ValidateRequest(ReviewRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Review request cannot be null");

            if (string.IsNullOrWhiteSpace(request.Message) || request.Message.Length < 5 || request.Message.Length > 1000)
                throw new ValidationException("Review message must be between 5 and 1000 characters");
        }
    }
}
