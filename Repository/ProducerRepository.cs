using IMDBApi_Assignment4.Models.DB;
using IMDBApi_Assignment4.Models.Enums;
using IMDBApi_Assignment4.Repository.Interface;

namespace IMDBApi_Assignment4.Repository
{
    public class ProducerRepository : BaseRepository<Person>, IProducerRepository
    {
        public ProducerRepository(IConfiguration configuration)
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
                AND Role = 'Producer'";

            var personRaw = await QuerySingleOrDefaultAsync(sql, new { Id = id });

            if (personRaw == null)
                return personRaw;

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
                WHERE Role = 'Producer'";

            var result = await QueryAsync(sql);

            return result.Select(r => new Person
            {
                Id = r.Id,
                Name = r.Name,
                Bio = r.Bio,
                Gender = (Gender)r.Gender,
                DOB = r.DOB
            }).ToList();
        }

        public async Task CreateAsync(Person producer)
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
                    @Gender,
                    'Producer'
                );
                SELECT CAST(SCOPE_IDENTITY() as int)";

            producer.Id = await ExecuteScalarAsync<int>(sql, new
            {
                producer.Name,
                producer.Bio,
                DOB = producer.DOB.ToDateTime(TimeOnly.MinValue),
                Gender = (int)producer.Gender
            });
        }

        public async Task<bool> UpdateAsync(Person producer)
        {
            const string sql = @"
                UPDATE Person
                SET 
                    Name = @Name,
                    Bio = @Bio,
                    DOB = @DOB,
                    GenderId = @Gender
                WHERE Id = @Id 
                AND Role = 'Producer'";

            var affected = await ExecuteAsync(sql, new
            {
                producer.Id,
                producer.Name,
                producer.Bio,
                DOB = producer.DOB.ToDateTime(TimeOnly.MinValue),
                Gender = (int)producer.Gender
            });

            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM Person 
                WHERE Id = @Id 
                AND Role = 'Producer'";

            return await ExecuteAsync(sql, new { Id = id }) > 0;
        }
    }
}
