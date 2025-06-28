using IMDBApi_Assignment4.Models.DTOs.Request;

namespace IMDBApi_Assignment4.Validations.Interface
{
    public interface IReviewValidation
    {
        void ValidateMovieIdAsync(int movieId);
        void ValidateIdAsync(int reviewId);
        void ValidateRequest(ReviewRequest request);
    }
}
