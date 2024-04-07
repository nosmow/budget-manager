using budget_manager.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace budget_manager.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                                        INSERT INTO category (name, type_operation_id, user_id)
                                        VALUES (@Name, @type_operation_id, @UserId);

                                        SELECT SCOPE_IDENTITY();", category);
            category.Id = id;
        }

        public async Task<IEnumerable<Category>> Get(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>("SELECT * FROM category WHERE user_id = @UserId", new { userId });
        }

        public async Task<Category> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>(
                @"SELECT * FROM category WHERE id = @Id AND user_id = @UserId", new { id, userId });
        }

        public async Task Update(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE category 
                    SET name = @Name, 
                    type_operation_id = @type_operation_id
                    WHERE id = @Id", category);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE category WHERE id = @Id", new { id });
        }
    }
}
