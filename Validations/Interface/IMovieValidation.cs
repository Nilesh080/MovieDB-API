using IMDBApi_Assignment4.Models.DTOs.Request;

namespace IMDBApi_Assignment4.Validations.Interface
{
    public interface IMovieValidation
    {
        Task ValidateActorIdsAsync(List<int> actorIds);
        Task ValidateGenreIdsAsync(List<int> genreIds);
        Task ValidateProducerIdAsync(int producerId);
        void ValidateRequest(MovieRequest request);
    }
}
