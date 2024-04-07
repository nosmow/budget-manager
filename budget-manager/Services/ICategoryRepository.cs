using budget_manager.Models;

namespace budget_manager.Services
{
    public interface ICategoryRepository
    {
        Task Create(Category category);
        Task Delete(int id);
        Task<IEnumerable<Category>> Get(int userId);
        Task<Category> GetById(int id, int userId);
        Task Update(Category category);
    }
}
