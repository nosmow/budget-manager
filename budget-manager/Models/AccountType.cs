using System.ComponentModel.DataAnnotations;

namespace budget_manager.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La longitud del campo {0} debe estar entre {2} y {1}")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Orden { get; set; }
    }
}
