using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.Enums;
using IMDBApi_Assignment4.Repository;
using IMDBApi_Assignment4.Repository.Interface;

namespace IMDBApi_Assignment4.Repositories
{
    public class ActorRepository : BaseRepository<Person>, IActorRepository
    {
        public ActorRepository(IConfiguration configuration)
            : base(configuration.GetConnectionString("IMDBDB"), "Person")
        {
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Name, 
                    Bio, 
                    DOB, 
                    GenderId as Gender
                FROM Person
                WHERE Id = @Id 
                AND Role = 'Actor'";

            var personRaw = await QuerySingleOrDefaultAsync(sql, new { Id = id });

            if (personRaw == null) return null;

            return new Person
            {
                Id = personRaw.Id,
                Name = personRaw.Name,
                Bio = personRaw.Bio,
                Gender = (Gender)personRaw.Gender,
                DOB = personRaw.DOB
            };
        }

        public async Task<List<Person>> GetAllAsync()
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Name, 
                    Bio, 
                    DOB, 
                    GenderId as Gender
                FROM Person
                WHERE Role = 'Actor'";

            var result = await QueryAsync(sql);

            return result.Select(r => new Person
            {
                Id = r.Id,
                Name = r.Name,
                Bio = r.Bio,
                Gender = (Gender)r.Gender,
                DOB = (r.DOB)
            }).ToList();
        }

        public async Task CreateAsync(Person actor)
        {
            const string sql = @"
                INSERT INTO Person (
                    Name,
                    Bio,
                    DOB,
                    GenderId,
                    Role
                )
                VALUES (
                    @Name,
                    @Bio,
                    @DOB,
                    @GenderId,
                    'Actor'
                );
                SELECT CAST(SCOPE_IDENTITY() as int);";

            actor.Id = await ExecuteScalarAsync<int>(sql, new
            {
                actor.Name,
                actor.Bio,
                DOB = actor.DOB.ToDateTime(TimeOnly.MinValue),
                GenderId = (int)actor.Gender
            });
        }

        public async Task<bool> UpdateAsync(Person actor)
        {
            const string sql = @"
                UPDATE Person
                SET 
                    Name = @Name,
                    Bio = @Bio,
                    DOB = @DOB,
                    GenderId = @GenderId
                WHERE Id = @Id 
                AND Role = 'Actor'";

            var affected = await ExecuteAsync(sql, new
            {
                actor.Id,
                actor.Name,
                actor.Bio,
                DOB = actor.DOB.ToDateTime(TimeOnly.MinValue),
                GenderId = (int)actor.Gender
            });

            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM Person 
                WHERE Id = @Id 
                AND Role = 'Actor'";

            return await ExecuteAsync(sql, new { Id = id }) > 0;
        }
    }
}
