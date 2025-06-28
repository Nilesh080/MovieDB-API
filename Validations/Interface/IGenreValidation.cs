using IMDBApi_Assignment4.Models.DTOs.Request;

namespace IMDBApi_Assignment4.Validations.Interface
{
    public interface IGenreValidation
    {
        void ValidateIdAsync(int id);
        void ValidateRequestAsync(GenreRequest request);
    }
}
