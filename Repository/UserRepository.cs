using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Repository.Interface;

namespace IMDBApi_Assignment4.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration)
                : base(configuration.GetConnectionString("IMDBDB"), "User")
        {
        }
        public async Task CreateAsync(User user)
        {
            const string sql = @"
                INSERT INTO Users (
                    FirstName, 
                    LastName, 
                    Email, 
                    Password
                )
                VALUES (
                    @FirstName, 
                    @LastName, 
                    @Email, 
                    @Password
                );
                SELECT CAST(SCOPE_IDENTITY() as int)";

            user.Id = await ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            const string sql = @"
                SELECT 
                    Id, 
                    FirstName, 
                    LastName, 
                    Email, 
                    Password
                FROM Users
                WHERE Email = @Email";

            return await QuerySingleOrDefaultAsync(sql, new { Email = email });
        }

        public async Task<List<User>> GetAllAsync()
        {
            const string sql = @"
                SELECT 
                    Id, 
                    FirstName, 
                    LastName, 
                    Email
                FROM Users";

            var users = await QueryAsync(sql);
            return users.ToList();
        }
    }
}