using AutoMapper;
using budget_manager.Models;

namespace budget_manager.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, CreationAccountViewModel>();
        }
    }
}