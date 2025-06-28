using IMDBApi_Assignment4.Models.DTOs;
using IMDBApi_Assignment4.Models.DTOs.Request;

namespace IMDBApi_Assignment4.Validations.Interface
{
    public interface IUserValidation
    {
        void ValidateSignupRequestAsync(SignUpRequest request);
        void ValidateLoginRequestAsync(LoginRequest request);
    }
}
