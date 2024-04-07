using System.ComponentModel.DataAnnotations;

namespace budget_manager.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Display(Name = "Tipo cuenta")]
        public int AccountTypeId { get; set; } 
        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Description { get; set;}
        public string AccountType { get; set; }
    }
}
