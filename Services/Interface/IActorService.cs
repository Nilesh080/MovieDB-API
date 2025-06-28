using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.DTOs.Request;
using IMDBApi_Assignment4.Models.DTOs.Response;

namespace IMDBApi_Assignment4.Services.Interface
{
    public interface IActorService
    {
        Task<List<PersonResponse>> GetAllAsync();
        Task<PersonResponse> GetByIdAsync(int id);
        Task<(string Message, int Id)> CreateAsync(PersonRequest request);
        Task<string> UpdateAsync(int id, PersonRequest request);
        Task DeleteAsync(int id);
        Task<Person> ExistsAsync(int id);
    }
}
