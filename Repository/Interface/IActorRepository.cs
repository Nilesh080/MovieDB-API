using IMDBApi_Assignment4.Models.DB;

namespace IMDBApi_Assignment4.Repository.Interface
{
    public interface IActorRepository
    {
        Task<List<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task CreateAsync(Person actor);
        Task<bool> UpdateAsync(Person actor);
        Task<bool> DeleteAsync(int id);
    }
}
