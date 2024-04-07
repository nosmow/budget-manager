using budget_manager.Models;

namespace budget_manager.Services
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
        Task Delete(int id);
        Task Edit(AccountType accountType);
        Task<bool> Exists(string name, int userId);
        Task<IEnumerable<AccountType>> Get(int userId);
        Task<AccountType> GetById(int id, int userId);
        Task Order(IEnumerable<AccountType> accountTypesOrder);
    }
}
