using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;

namespace IMDBApi_Assignment4.Services.Interface
{
    public interface IUserService
    {
        Task<string> SignupAsync(SignUpRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<List<UserResponse>> GetAllAsync();
        Task<User> ExistsAsync(string email);
    }
}