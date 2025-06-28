using IMDBApi_Assignment4.Models.DTOs.Request;

namespace IMDBApi_Assignment4.Validations.Interface
{
    public interface IProducerValidation
    {
        void ValidateIdAsync(int id);
        void ValidateRequest(PersonRequest request);
    }
}
