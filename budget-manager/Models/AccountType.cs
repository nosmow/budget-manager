using budget_manager.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace budget_manager.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [FirstCapitalLetter]
        [Remote(action: "VerifyExistsAccountType", controller: "AccountType")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Orden { get; set; }
    }
}
