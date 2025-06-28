using IMDBApi_Assignment4.Models.DTOs.Request;

namespace IMDBApi_Assignment4.Validations.Interface
{
    public interface IActorValidation
    {
        public void ValidateIdAsync(int id);
        public void ValidateRequest(PersonRequest request);
    }
}
