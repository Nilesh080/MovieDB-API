using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations.Interface;

namespace IMDBApi_Assignment4.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMovieService _movieService;
        private readonly IReviewValidation _reviewValidation;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IMovieService movieService, IReviewValidation reviewValidation, IReviewRepository reviewRepository)
        {
            _movieService = movieService;
            _reviewValidation = reviewValidation;
            _reviewRepository = reviewRepository;
        }
        public async Task<List<ReviewResponse>> GetByMovieIdAsync(int movieId)
        {
            _reviewValidation.ValidateMovieIdAsync(movieId);
            await _movieService.ExistsAsync(movieId);

            var reviewIds = await _reviewRepository.GetByMovieIdAsync(movieId);

            var reviewTasks = reviewIds.Select(async reviewId =>
            {
                var review = await _reviewRepository.GetByIdAsync(reviewId);
                return MapToResponse(review);
            });

            var reviewResponses = await Task.WhenAll(reviewTasks);
            return reviewResponses.ToList();
        }

        public async Task<ReviewResponse> GetByIdAsync(int id, int movieId)
        {
            _reviewValidation.ValidateMovieIdAsync(movieId);
            await _movieService.ExistsAsync(movieId);

            _reviewValidation.ValidateIdAsync(id);
            await ExistsAsync(id);

            await ValidateReviewBelongsToMovieAsync(id, movieId);

            var review = await _reviewRepository.GetByIdAsync(id);

            return MapToResponse(review);
        }

        public async Task<(string Message, int Id)> CreateAsync(int movieId, ReviewRequest request)
        {
            _reviewValidation.ValidateMovieIdAsync(movieId);
            await _movieService.ExistsAsync(movieId);
            _reviewValidation.ValidateRequest(request);

            var review = new Review
            {
                Message = request.Message,
                MovieId = movieId
            };

            await _reviewRepository.CreateAsync(review);

            return ($"Review for Movie Id: {movieId} created successfully.", review.Id);
        }

        public async Task<string> UpdateAsync(int movieId, int id, ReviewRequest request)
        {
            _reviewValidation.ValidateMovieIdAsync(movieId);
            await _movieService.ExistsAsync(movieId);
            _reviewValidation.ValidateIdAsync(id);
            await ExistsAsync(id);
            await ValidateReviewBelongsToMovieAsync(id, movieId);
            _reviewValidation.ValidateRequest(request);

            var existingReview = await _reviewRepository.GetByIdAsync(id);

            existingReview.Message = request.Message;
            existingReview.MovieId = movieId;

            await _reviewRepository.UpdateAsync(existingReview);

            return $"Review Id: {id} updated successfully.";
        }

        public async Task DeleteAsync(int movieId, int id)
        {
            _reviewValidation.ValidateMovieIdAsync(movieId);
            await _movieService.ExistsAsync(movieId);
            _reviewValidation.ValidateIdAsync(id);
            await ExistsAsync(id);
            await ValidateReviewBelongsToMovieAsync(id, movieId);

            await _reviewRepository.DeleteAsync(id);
        }

        public async Task DeleteAllByMovieIdAsync(int movieId)
        {
            _reviewValidation.ValidateMovieIdAsync(movieId);
            await _movieService.ExistsAsync(movieId);

            await _reviewRepository.DeleteByMovieIdAsync(movieId);
        }

        public async Task<Review> ExistsAsync(int id)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);

            if (existingReview == null)
            {
                throw new KeyNotFoundException($"Review with ID {id} not found");
            }

            return existingReview;
        }

        public async Task ValidateReviewBelongsToMovieAsync(int id, int movieId)
        {
            var reviewIdsForMovie = (await GetByMovieIdAsync(movieId))
                            .Select(r => r.Id)
                            .ToList();

            if (!reviewIdsForMovie.Contains(id))
            {
                throw new InvalidOperationException($"Review with ID {id} does not belong to Movie with ID {movieId}");
            }
        }

        private ReviewResponse MapToResponse(Review review)
        {
            return new ReviewResponse
            {
                Id = review.Id,
                Message = review.Message
            };
        }
    }
}
