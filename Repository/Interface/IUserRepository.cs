using IMDBApi_Assignment4.Models.DB;

namespace IMDBApi_Assignment4.Repository.Interface
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
    }
}