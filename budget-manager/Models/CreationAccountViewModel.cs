using Microsoft.AspNetCore.Mvc.Rendering;

namespace budget_manager.Models
{
    public class CreationAccountViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountType { get; set; }
    }
}
