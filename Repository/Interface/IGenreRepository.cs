using IMDBApi_Assignment4.Models.DB;

namespace IMDBApi_Assignment4.Repository.Interface
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task CreateAsync(Genre genre);
        Task<bool> UpdateAsync(Genre genre);
        Task<bool> DeleteAsync(int id);
    }
}
