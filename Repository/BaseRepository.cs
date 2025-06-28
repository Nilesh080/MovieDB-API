using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace IMDBApi_Assignment4.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;

        protected BaseRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<TResult>(sql, parameters);
        }

        protected async Task<IEnumerable<T>> QueryAsync(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(sql, param);
        }

        protected async Task<T> QuerySingleOrDefaultAsync(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, param);
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<T>(sql, param);
        }

        protected async Task ExecuteStoredProcedureAsync(string spName, DynamicParameters parameters)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}