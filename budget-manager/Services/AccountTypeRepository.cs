using budget_manager.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace budget_manager.Services
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly string connectionString;

        public AccountTypeRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection"); 
        }

        public async Task Create(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                                    ("account_type_insert", new { userId = accountType.UserId,
                                                    name = accountType.Name},
                                                    commandType: CommandType.StoredProcedure);
            accountType.Id = id;
        }

        public async Task<bool> Exists(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>
                                                                        (@"SELECT 1 
                                                                        FROM account_type
                                                                        WHERE name = @Name AND user_id = @UserId;", 
                                                                        new {name, userId});

            return exists == 1;
        }

        public async Task<IEnumerable<AccountType>> Get(int userId)
        {
            using var connection = new SqlConnection (connectionString);
            return await connection.QueryAsync<AccountType>(@"SELECT id, name, orden
                                                            FROM account_type
                                                            WHERE user_id = @UserId
                                                            ORDER BY orden;", 
                                                            new {userId});
        }

        public async Task Edit(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE account_type
                                          SET name = @Name
                                          WHERE id = @Id;", accountType);
        }

        public async Task<AccountType> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>(
                                                                @"SELECT id, name, orden
                                                                FROM account_type
                                                                WHERE id = @Id AND user_id = @UserId;",
                                                                new {id, userId});
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE account_type WHERE id = @Id;", new {id});
        }

        public async Task Order(IEnumerable<AccountType> accountTypesOrder)
        {
            var query = "UPDATE account_type SET orden = @Orden WHERE id = @ID;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, accountTypesOrder);
        }
    }
}
