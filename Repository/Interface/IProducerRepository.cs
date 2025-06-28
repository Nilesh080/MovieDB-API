using IMDBApi_Assignment4.Models.DB;

namespace IMDBApi_Assignment4.Repository.Interface
{
    public interface IProducerRepository
    {
        Task<List<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task CreateAsync(Person producer);
        Task<bool> UpdateAsync(Person producer);
        Task<bool> DeleteAsync(int id);
    }
}
