using budget_manager.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_manager.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Account account)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO account(name, account_type_id, balance, description)
                    VALUES(@Name, @AccountTypeId, @Balance, @Description);
                
                    SELECT SCOPE_IDENTITY();", account);

            account.Id = id;
        }

        public async Task<IEnumerable<Account>> Search(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Account>(
                                    @"SELECT c.id, c.name, balance, ct.name AS accountType
                                    FROM account c
                                    INNER JOIN account_type ct
                                    ON ct.id = c.account_type_id
                                    WHERE ct.user_id = @UserId
                                    ORDER By ct.orden;", new { userId });
        }

        public async Task<Account> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>(
                                    @"SELECT c.id, c.name, balance, description, ct.id
                                    FROM account c
                                    INNER JOIN account_type ct
                                    ON ct.id = c.account_type_id
                                    WHERE ct.user_id = @UserId AND c.id = @Id", new { id, userId });
        }

        public async Task Update(CreationAccountViewModel account)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                            @"UPDATE account
                            SET name = @Name, balance = @Balance,
                            description = @Description, account_type_id = @AccountTypeId
                            WHERE id = @Id;", account);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE account WHERE id = @Id", new { id });
        }
    }
}